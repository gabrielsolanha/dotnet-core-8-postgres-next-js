using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Mapper;
using AplicacaoWeb.Mapper.Interface;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Dtos.Banner;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Service.Interfaces;

namespace AplicacaoWeb.Aplication
{
    public class BannersApp : IBannersApp
    {
        private readonly IBannerRepository bannerRepository;
        private readonly IImageHandlerService imageHandler;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISaveFileService iSaveFileService;

        public BannersApp(IBannerRepository bannerRepository,
            IUnitOfWork unitOfWork,
            IImageHandlerService imageHandler,
            ISaveFileService iSaveFileService
            )
        {
            this.bannerRepository = bannerRepository ?? throw new ArgumentNullException(nameof(bannerRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler));
            this.iSaveFileService = iSaveFileService ?? throw new ArgumentNullException(nameof(iSaveFileService));
        }
        public async Task<BannerDto> Add(BannerDto bannerDto, string changeMaker)
        {
            try
            {
                var mapper = new BannerMapper();
                Banner banner = mapper.MapperFromDto(bannerDto);
                banner.CreatedBy = changeMaker;
                banner.CreatedAt = DateTime.UtcNow;
                bannerRepository.Add(banner);
                await unitOfWork.SaveAsync();

                return mapper.MapperToDto(banner);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public async Task<BannerDto> Add(BannerWithArquiveDto bannerDtoAdd, string changeMaker)
        {
            try
            {
                BannerDto bannerDto;
                bannerDto = ToDtoWithoutArquive(bannerDtoAdd);
                unitOfWork.BeginTransaction();
                bannerDtoAdd.Id = bannerDto.Id;
                if (bannerDtoAdd.ImageFile != null)
                {
                    if(!(await imageHandler.IsImageDimensionsValidAsync(bannerDtoAdd.ImageFile, bannerDtoAdd.IsMobile)))
                    {
                        throw new Exception($"Houve um erro ao fazer a operação: Dimenções da imagem Inválidas! Favor colocar 1800 x 600 Desktop e 750 x 1100 Mobile");
                    }
                }
                else
                {
                    throw new Exception($"Houve um erro ao fazer a operação: Nenhuma imagem identificada!");
                }
                bannerDto = await GeneratePath(bannerDtoAdd);
                bannerDto = await Add(bannerDto, changeMaker);
                unitOfWork.Commit();

                return bannerDto;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        private async Task<BannerDto> GeneratePath(BannerWithArquiveDto banner)
        {
            List<ImageUrlAndNameDto> images = new List<ImageUrlAndNameDto>();
            IFormFile image = banner.ImageFile;
            string Path = await iSaveFileService.ImageWebpToLocalAsync(image);
            return new BannerDto()
            {
                Id = banner.Id,
                Title = banner.Title,
                IsDeleted = banner.IsDeleted,
                IsMobile = banner.IsMobile,
                Description = banner.Description,
                RedirectLink = banner.RedirectLink,
                ArquiveName = banner.ImageFile?.Name,
                Path = Path,
            };
        }

        private BannerDto ToDtoWithoutArquive(BannerWithArquiveDto banner)
        {
            return new BannerDto()
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                RedirectLink = banner.RedirectLink,
                IsDeleted = banner.IsDeleted,
                ArquiveName = banner.ImageFile?.Name,
                IsMobile = banner.IsMobile,
            };
        }
        private BannerWithArquiveDto FromDtoToAquiveDtoWithoutArquive(BannerDto banner)
        {
            return new BannerWithArquiveDto()
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                RedirectLink = banner.RedirectLink,
                IsDeleted = banner.IsDeleted,
                IsMobile = (bool) banner.IsMobile,
            };
        }

        public async Task Delete(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                Banner? banner = bannerRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();
                if (banner == null)
                {
                    throw new Exception("Nenhum banner encontrado.");
                }
                string? path = banner?.Path;
                await iSaveFileService.DeleteFromLocalAsync(path);
                bannerRepository.Delete(banner);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                return;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public BannerDto Get(int id)
        {
            var banner = bannerRepository.GetBannersById(id);

            if (banner == null)
            {
                throw new Exception("Nenhum banner encontrado.");
            }
            var mapper = new BannerMapper();


            return mapper.MapperToDto(banner);
        }

        public BannerWithArquiveDto GetComplete(int id)
        {
            var banner = Get(id);
            BannerWithArquiveDto bannerWithArquive = FromDtoToAquiveDtoWithoutArquive(banner);
            bannerWithArquive.ImageFileStrem = iSaveFileService.GetFileStream(banner.Path);
            return bannerWithArquive;
        }

        public IEnumerable<BannerDto> List(PaginationDto<BannerDto> filtro)
        {
            var source = bannerRepository.GetAllWhen(x => (!filtro.Filter.Id.HasValue || x.Id == filtro.Filter.Id) &&
                                                         (!filtro.Filter.IsMobile.HasValue || x.IsMobile == filtro.Filter.IsMobile) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Description) || x.Description.ToLower().Contains(filtro.Filter.Description.ToLower())) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.RedirectLink) || x.RedirectLink.ToLower().Contains(filtro.Filter.RedirectLink.ToLower())) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Title) || x.Title.ToLower().Contains(filtro.Filter.Title.ToLower())) &&
                                                         x.IsDeleted == filtro.Filter.IsDeleted);
            return MapFilter(source, filtro);

        }

        public IEnumerable<BannerWithArquiveDto> ListAllCompleteToFront(bool isMobile)
        {
            BannerDto bannerDto = new BannerDto() 
            {
                IsMobile = isMobile
            };
            PaginationDto<BannerDto> filtro = new PaginationDto<BannerDto>() 
            {
                Filter = bannerDto,
                Descending = true,
                ItemCount = 100,
                SortOrder = "",
                Page = 0,
                RelParam = "",


            };
            IEnumerable<BannerDto> frontList = List(filtro);
            foreach (var item in frontList)
            {
                yield return GetComplete((int)item.Id);
            }
        }

        private IEnumerable<BannerDto> MapFilter(IEnumerable<Banner> banners, PaginationDto<BannerDto> filtro)
        {
            var paginacao = Pagination(banners, filtro);
            var mapper = new BannerMapper();

            foreach (var item in paginacao)
            {
                yield return mapper.MapperToDto(item);
            }
        }
        private IEnumerable<Banner> Pagination(IEnumerable<Banner> banners, PaginationDto<BannerDto> filtro)
        {

            var total = banners.Count();
            banners = banners.Skip((filtro.Page) * filtro.ItemCount).Take(filtro.ItemCount);
            filtro.ItemCount = total;

            return banners;
        }

        public async Task<BannerDto> Update(int id, BannerWithArquiveDto bannerdtoArquive, string changeMaker)
        {

            var mapper = new BannerMapper();
            var existingBanner = bannerRepository.GetBannersById(id);
            if (existingBanner == null)
            {
                throw new Exception("Nenhum banner encontrado.");
            }
            try
            {
                BannerDto bannerDto = new BannerDto();
                unitOfWork.BeginTransaction();

                if (bannerdtoArquive.ImageFile != null)
                {
                    bannerDto = await GeneratePath(bannerdtoArquive);
                }
                else
                {
                    bannerDto = new BannerDto()
                    {
                        Description = bannerdtoArquive.Description,
                        RedirectLink = bannerdtoArquive.RedirectLink,
                        Id = bannerdtoArquive.Id,
                        Title = bannerdtoArquive.Title,
                        IsDeleted = bannerdtoArquive.IsDeleted,
                        IsMobile = bannerdtoArquive.IsMobile,
                    };
                }
                bannerDto = await Update(id, bannerDto, changeMaker);
                unitOfWork.Commit();
                return bannerDto;

            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public async Task<BannerDto> Update(int id, BannerDto bannerdto, string changeMaker)
        {

            var mapper = new BannerMapper();
            var existingBanner = bannerRepository.GetBannersById(id);
            Banner banner = mapper.MapperFromDtoToUpdate(bannerdto, existingBanner);
            banner.UpdatedDate = DateTime.UtcNow;
            banner.UpdatedBy = changeMaker;
            bannerRepository.Update(banner);
            await unitOfWork.SaveAsync();
            return mapper.MapperToDto(banner);
        }
    }
}
