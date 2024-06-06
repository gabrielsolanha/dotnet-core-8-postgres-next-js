using System.Linq.Expressions;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAsync();
        IEnumerable<T> GetAllWhen(Expression<Func<T, bool>> predicado);
        Task<IEnumerable<T>> GetAll();

    }
}
