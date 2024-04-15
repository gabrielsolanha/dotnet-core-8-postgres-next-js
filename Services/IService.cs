namespace AplicacaoWeb.Services
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> List();
        Task<T> Get(int id);
        Task<T> Add(T obj);
        Task Update(int id, T obj);
        Task Delete(int id);
    }
}
