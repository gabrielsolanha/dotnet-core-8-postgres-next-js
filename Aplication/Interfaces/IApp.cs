using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IApp<T> where T : BaseDto
    {
        IEnumerable<T> List(PaginationDto<T> obj);
        T Get(int id);
        Task<T> Add(T obj);
        Task<T> Update(int id, T obj);
        Task Delete(int id);
    }
}
