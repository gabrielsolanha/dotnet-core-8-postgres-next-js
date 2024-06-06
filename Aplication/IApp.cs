using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Aplication
{
    public interface IApp<T> where T : BaseDto
    {
        IEnumerable<T> List(PaginationDto<T> obj);
        Task<T> Get(int id);
        Task<T> Add(T obj);
        Task<T> Update(int id, T obj);
        Task Delete(int id);
    }
}
