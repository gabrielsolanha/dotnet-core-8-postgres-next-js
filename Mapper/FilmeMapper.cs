using AplicacaoWeb.Mapper.Interface;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Mapper
{
    public class FilmeMapper : IMapper<FilmeDto, Filme>
    {
        public Filme MapperFromDto(FilmeDto dto)
        {
            Filme filme = new Filme() { 
                CategoryId = (int) dto.CategoryId,
                Description = dto.Description,
                Rodagem = dto.Rodagem ?? 0,
                Id = dto.Id ?? 0,
                Title = dto.Title,
                Plate = dto.Plate,
                UserResponsibleId = (int) dto.UserResponsibleId,
                Images = new List<ImageUrlAndName>(),
                IsDeleted = dto.IsDeleted
            };
            if(dto.Images != null)
            {
                List<ImageUrlAndName> images = new List<ImageUrlAndName>();
                foreach (ImageUrlAndNameDto image in dto.Images)
                {
                    images.Add(new ImageUrlAndName()
                    {
                        ArquiveName = image.ArquiveName,
                        Id = image.Id ?? 0,
                        Url = image.Url,
                        FilmeId = image.FilmeId,
                        IsDeleted = image.IsDeleted
                    });
                }
                filme.Images = images;
            }
            return filme;
        }

        public Filme MapperFromDtoToUpdate(FilmeDto dto, Filme currentValue)
        {
            Filme filme = new Filme()
            {
                CategoryId = (int)dto.CategoryId,
                Description = dto.Description,
                Rodagem = dto.Rodagem ?? 0,
                Id = currentValue.Id,
                Title = dto.Title,
                Plate = dto.Plate,
                UserResponsibleId = (int)dto.UserResponsibleId,
                Images = currentValue.Images,
                IsDeleted = dto.IsDeleted,
                CreatedAt = currentValue.CreatedAt,
                CreatedBy = currentValue.CreatedBy,
                
            };
            return filme;
        }

        public FilmeDto MapperToDto(Filme entity)
        {
            FilmeDto filme = new FilmeDto()
            {
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Rodagem = entity.Rodagem,
                Id = entity.Id,
                Title = entity.Title,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                Plate = entity.Plate,
                UserResponsibleId = entity.UserResponsibleId,
                Images = new List<ImageUrlAndNameDto>(),
                IsDeleted = entity.IsDeleted
            };
            if(entity.Images != null)
            {
                List<ImageUrlAndNameDto> images = new List<ImageUrlAndNameDto>();
                foreach (ImageUrlAndName image in entity.Images)
                {
                    images.Add(new ImageUrlAndNameDto()
                    {
                        ArquiveName = image.ArquiveName,
                        Id = image.Id,
                        Url = image.Url,
                        FilmeId = image.FilmeId,
                        IsDeleted = image.IsDeleted
                    });
                }
                filme.Images = images;
            }
            return filme;
        }
    }
}
