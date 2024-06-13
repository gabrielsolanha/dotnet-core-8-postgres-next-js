using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Mapper;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Aplication
{
    public class FilmesApp : IFilmesApp
    {
        private readonly IFilmeRepository filmeRepository;
        private readonly IRepositoryBase<User> userRepository;
        private readonly IRepositoryBase<ImageUrlAndName> _imageUrlRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IS3Service iS3Service;

        public FilmesApp(IFilmeRepository filmeRepository,
            IUnitOfWork unitOfWork, 
            IRepositoryBase<User> userRepository,
            IRepositoryBase<ImageUrlAndName> _imageUrlRepository,
            IS3Service iS3Service
            )
        {
            this.filmeRepository = filmeRepository ?? throw new ArgumentNullException(nameof(filmeRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._imageUrlRepository = _imageUrlRepository ?? throw new ArgumentNullException(nameof(_imageUrlRepository));
            this.iS3Service = iS3Service ?? throw new ArgumentNullException(nameof(iS3Service));
        }
        public async Task<FilmeDto> Add(FilmeDto filmeDto)
        {
            try
            {
                var mapper = new FilmeMapper();
                Filme filme = mapper.MapperFromDto(filmeDto);
                filme.CreatedBy = "Faze de teste";
                filme.CreatedAt = DateTime.UtcNow;
                filmeRepository.Add(filme);
                await unitOfWork.SaveAsync();

                return mapper.MapperToDto(filme);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }
        public async Task AddImagesOnDB(FilmeDto filme)
        {
            foreach(ImageUrlAndNameDto image in filme.Images)
            {
                ImageUrlAndName i = new ImageUrlAndName()
                {
                    Url = image.Url,
                    ArquiveName = image.ArquiveName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "faze de teste",
                    FilmeId = filme.Id ?? throw new Exception($"Houve um erro ao fazer a operação: Falha ao recuperar ID do filme"),
                    IsDeleted = filme.IsDeleted,
                };
                _imageUrlRepository.Add(i);
            }
            await unitOfWork.SaveAsync();
        }
        public async Task<FilmeDto> Add(FilmeWithArquiveDto filmeDtoAdd)
        {
            try
            {
                FilmeDto filmeDto;
                filmeDto = ToDtoWithoutArquive(filmeDtoAdd);
                unitOfWork.BeginTransaction();
                filmeDto = await Add(filmeDto);
                filmeDtoAdd.Id = filmeDto.Id;
                filmeDto = await GenerateAWSLink(filmeDtoAdd);
                await AddImagesOnDB(filmeDto);
                unitOfWork.Commit();

                return filmeDto;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        private async Task<FilmeDto> GenerateAWSLink(FilmeWithArquiveDto filme)
        {
            List<ImageUrlAndNameDto> images = new List<ImageUrlAndNameDto>();
            foreach(IFormFile image in filme.ImageFiles)
            {
                ImageUrlAndNameDto temp = new ImageUrlAndNameDto
                {
                    ArquiveName = image.Name,
                    FilmeId = filme.Id ?? throw new Exception($"Houve um erro ao fazer a operação: falha ao recuperar ID do filme"),
                    Url = await iS3Service.ImageWebpToS3Async(image),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Fase de teste",

                };
                images.Add(temp);
            }

            return new FilmeDto()
            {
                CategoryId = filme.CategoryId,
                Description = filme.Description,
                Id = filme.Id,
                Title = filme.Title,
                Plate = filme.Plate,
                UserResponsibleId = filme.UserResponsibleId,
                Images = images,
                IsDeleted = filme.IsDeleted
            };
        }

        private FilmeDto ToDtoWithoutArquive(FilmeWithArquiveDto filme)
        {
            return new FilmeDto()
            {
                CategoryId = filme.CategoryId,
                Description = filme.Description,
                Id = filme.Id,
                Title = filme.Title,
                Plate = filme.Plate,
                UserResponsibleId = filme.UserResponsibleId,
                IsDeleted = filme.IsDeleted
            };
        }

        public async Task Delete(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                Filme filme = filmeRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();
                if (filme == null)
                {
                    throw new Exception("Nenhum filme encontrado.");
                }
                foreach (ImageUrlAndName deletar in filme.Images)
                {
                    ImageUrlAndName db = _imageUrlRepository.GetAllWhen(x => x.Id == deletar.Id).FirstOrDefault() ?? throw new Exception($"Houve um erro ao fazer a operação: Falha ao recuperar Imagem no data base");
                    await iS3Service.DeleteFromS3Async(db.Url);
                    _imageUrlRepository.Delete(db);
                }
                filmeRepository.Delete(filme);
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

        public async Task<FilmeDto> Get(int id)
        {
            Filme filme = filmeRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();

            if (filme == null)
            {
                throw new Exception("Nenhum filme encontrado.");
            }
            var mapper = new FilmeMapper();


            return mapper.MapperToDto(filme);
        }

        public IEnumerable<FilmeDto> List(PaginationDto<FilmeDto> filtro)
        {
            var source = filmeRepository.GetAllWhen(x => (!filtro.Filter.Id.HasValue || x.Id == filtro.Filter.Id) &&
                                                         (!filtro.Filter.UserResponsibleId.HasValue || x.UserResponsibleId == filtro.Filter.UserResponsibleId) &&
                                                         (!filtro.Filter.CategoryId.HasValue || x.CategoryId == filtro.Filter.CategoryId) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Plate) || x.Plate == filtro.Filter.Plate) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Description) || x.Description.ToLower().Contains(filtro.Filter.Description.ToLower())) &&
                                                         (string.IsNullOrEmpty(filtro.Filter.Title) || x.Title.ToLower().Contains(filtro.Filter.Title.ToLower())) &&
                                                         x.IsDeleted == filtro.Filter.IsDeleted);
            return MapFilter(source, filtro);
            
        }

        private IEnumerable<FilmeDto> MapFilter(IEnumerable<Filme> filmes, PaginationDto<FilmeDto> filtro)
        {
            var paginacao = Pagination(filmes, filtro);
            var mapper = new FilmeMapper();

            foreach (var item in paginacao)
            {
                yield return mapper.MapperToDto(item);
            }
        }
        private IEnumerable<Filme> Pagination(IEnumerable<Filme> filmes, PaginationDto<FilmeDto> filtro)
        { 

            var total = filmes.Count();
            filmes = filmes.Skip((filtro.Page) * filtro.ItemCount).Take(filtro.ItemCount);
            filtro.ItemCount = total;

            return filmes;
        }

        public async Task<FilmeDto> Update(int id, FilmeWithArquiveDto filmedtoArquive)
        {

            var mapper = new FilmeMapper();
            var existingFilme = await filmeRepository.GetFilmesByIdAsync(id);
            if (existingFilme == null)
            {
                throw new Exception("Nenhum filme encontrado.");
            }
            try
            {
                FilmeDto filmeDto = new FilmeDto();
                unitOfWork.BeginTransaction();
                try
                {
                    if (filmedtoArquive.ImageFilesToDelete != null)
                    {
                        foreach (int deletar in filmedtoArquive.ImageFilesToDelete)
                        {
                            ImageUrlAndName db = _imageUrlRepository.GetAllWhen(x => x.Id == deletar).FirstOrDefault() ?? throw new Exception($"Houve um erro ao fazer a operação: Falha ao recuperar Imagem no data base");
                            await iS3Service.DeleteFromS3Async(db.Url);
                            _imageUrlRepository.Delete(db);
                        }
                    }

                    if (filmedtoArquive.ImageFiles != null)
                    {
                        filmeDto = await GenerateAWSLink(filmedtoArquive);
                        await AddImagesOnDB(filmeDto);
                    }
                    else
                    {
                        filmeDto = new FilmeDto()
                        {
                            CategoryId = filmedtoArquive.CategoryId,
                            Description = filmedtoArquive.Description,
                            Id = filmedtoArquive.Id,
                            Title = filmedtoArquive.Title,
                            Plate = filmedtoArquive.Plate,
                            UserResponsibleId = filmedtoArquive.UserResponsibleId,
                            IsDeleted = filmedtoArquive.IsDeleted
                        };
                    }
                    filmeDto = await Update(id, filmeDto);
                    unitOfWork.Commit();
                    return filmeDto;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!filmeRepository.FilmeExists(id))
                    {
                        throw new Exception("Nenhum filme encontrado.");
                    }
                    else
                    {
                        throw;
                    }
                    }
                }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public async Task<FilmeDto> Update(int id, FilmeDto filmedto)
        {

            var mapper = new FilmeMapper();
            var existingFilme = await filmeRepository.GetFilmesByIdAsync(id);
            Filme filme = mapper.MapperFromDtoToUpdate(filmedto, existingFilme);
            filme.UpdatedDate = DateTime.UtcNow;
            filme.UpdatedBy = "faze de teste";
            filmeRepository.Update(filme);
            await unitOfWork.SaveAsync();
            return mapper.MapperToDto(filme);
        }
    }
}
