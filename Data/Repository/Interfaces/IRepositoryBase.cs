namespace AplicacaoWeb.Data.Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        Task<bool> SaveAsync();
    }
}
