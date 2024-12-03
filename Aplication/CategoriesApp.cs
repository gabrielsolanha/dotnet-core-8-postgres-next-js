using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Data.UnitOfWork;
using AplicacaoWeb.Mapper;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Aplication
{
    public class CategoriesApp : IApp<CategoryDto>
    {
        private readonly IRepositoryBase<Category> categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoriesApp(
            IRepositoryBase<Category> categoryRepository,
            IUnitOfWork unitOfWork
            )
        {
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<CategoryDto> Add(CategoryDto categoryDto, string changeMaker)
        {
            try
            {
                unitOfWork.BeginTransaction();
                var mapper = new CategoryMapper();
                Category category = mapper.MapperFromDto(categoryDto);
                category.CreatedBy = changeMaker;
                category.CreatedAt = DateTime.UtcNow;
                categoryRepository.Add(category);
                await unitOfWork.SaveAsync();

                return mapper.MapperToDto(category);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                var category = categoryRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();
                if (category == null)
                {
                    throw new Exception("Nenhum category encontrado.");
                }
                categoryRepository.Delete(category);
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

        public CategoryDto Get(int id)
        {
            var category = categoryRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();

            if (category == null)
            {
                throw new Exception("Nenhum category encontrado.");
            }
            var mapper = new CategoryMapper();


            return mapper.MapperToDto(category);
        }

        public IEnumerable<CategoryDto> List(PaginationDto<CategoryDto> filtro)
        {
            var source = categoryRepository.GetAllWhen(x => (
                                                         !filtro.Filter.Id.HasValue || x.Id == filtro.Filter.Id) &&
                                                         x.IsDeleted == filtro.Filter.IsDeleted &&
                                                         (string.IsNullOrEmpty(filtro.Filter.CategoryName) || x.CategoryName.ToLower().Contains(filtro.Filter.CategoryName.ToLower()))
                                                         );
            return MapFilter(source, filtro);

        }

        private IEnumerable<CategoryDto> MapFilter(IEnumerable<Category> categories, PaginationDto<CategoryDto> filtro)
        {
            var paginacao = Pagination(categories, filtro);
            var mapper = new CategoryMapper();

            foreach (var item in paginacao)
            {
                yield return mapper.MapperToDto(item);
            }
        }
        private IEnumerable<Category> Pagination(IEnumerable<Category> categories, PaginationDto<CategoryDto> filtro)
        {

            var total = categories.Count();
            categories = categories.Skip((filtro.Page) * filtro.ItemCount).Take(filtro.ItemCount);
            filtro.ItemCount = total;

            return categories;
        }

        public async Task<CategoryDto> Update(int id, CategoryDto categorydto, string changeMaker)
        {

            var mapper = new CategoryMapper();
            var existingCategory = categoryRepository.GetAllWhen(x => x.Id == id).FirstOrDefault();
            if (existingCategory == null)
            {
                throw new Exception("Nenhum category encontrado.");
            }
            try
            {
                CategoryDto categoryDto = new CategoryDto();
                unitOfWork.BeginTransaction();

                var category = mapper.MapperFromDtoToUpdate(categorydto, existingCategory);

                category.UpdatedDate = DateTime.UtcNow;
                category.UpdatedBy = changeMaker;

                categoryRepository.Update(category);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                categoryDto = mapper.MapperToDto(category);
                return categoryDto;

            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception($"Houve um erro ao fazer a operação: {e.Message}");
            }
        }
    }
}
