using AplicacaoWeb.Mapper.Interface;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Dtos.Banner;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Mapper
{
    public class BannerMapper : IMapper<BannerDto, Banner>
    {
        public Banner MapperFromDto(BannerDto dto)
        {
            Banner filme = new Banner() { 
                Description = dto.Description,
                Id = dto.Id ?? 0,
                Title = dto.Title,
                IsDeleted = dto.IsDeleted,
                Path = dto.Path,
                ArquiveName = dto.ArquiveName,
                IsMobile = (bool) dto.IsMobile,
                RedirectLink = dto.RedirectLink,
            };
            return filme;
        }

        public Banner MapperFromDtoToUpdate(BannerDto dto, Banner currentValue)
        {
            Banner filme = new Banner()
            {
                Description = dto.Description,
                Id = currentValue.Id,
                Title = dto.Title,
                IsDeleted = dto.IsDeleted,
                CreatedAt = currentValue.CreatedAt,
                CreatedBy = currentValue.CreatedBy,
                Path = dto.Path ?? currentValue.Path,
                ArquiveName = dto.ArquiveName,
                IsMobile = (bool)dto.IsMobile,
                RedirectLink = dto.RedirectLink,
            };
            return filme;
        }

        public BannerDto MapperToDto(Banner entity)
        {
            BannerDto filme = new BannerDto()
            {
                Description = entity.Description,
                Id = entity.Id,
                Title = entity.Title,
                RedirectLink = entity.RedirectLink,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                IsDeleted = entity.IsDeleted,
                Path = entity.Path,
                ArquiveName= entity.ArquiveName,
                IsMobile = entity.IsMobile,
            };
            return filme;
        }
    }
}
