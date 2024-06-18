using AplicacaoWeb.Mapper.Interface;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Mapper
{
    public class CategoryMapper : IMapper<CategoryDto, Category>
    {
        public Category MapperFromDto(CategoryDto dto)
        {
            Category category = new Category() { 
                Id = dto.Id ?? 0,
                IsDeleted = dto.IsDeleted,
                CategoryName = dto.CategoryName
            };
            return category;
        }

        public Category MapperFromDtoToUpdate(CategoryDto dto, Category currentValue)
        {
            Category category = new Category()
            {
                Id = currentValue.Id,
                IsDeleted = dto.IsDeleted,
                CreatedAt = currentValue.CreatedAt,
                CreatedBy = currentValue.CreatedBy,
                CategoryName = dto.CategoryName,
                
            };
            return category;
        }

        public CategoryDto MapperToDto(Category entity)
        {
            CategoryDto category = new CategoryDto()
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate,
                IsDeleted = entity.IsDeleted,
                CategoryName = entity.CategoryName,
            };
            return category;
        }
    }
}
