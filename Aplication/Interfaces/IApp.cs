using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IApp<T> where T : BaseDto
    {
        IEnumerable<T> List(PaginationDto<T> obj);
        T Get(int id);
        Task<T> Add(T obj, string changeMaker);
        Task<T> Update(int id, T obj, string changeMaker);
        Task Delete(int id);
    }
}
