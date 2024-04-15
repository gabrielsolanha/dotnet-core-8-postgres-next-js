using AplicacaoWeb.Models;

namespace AplicacaoWeb.Data.Repository.Interfaces
{
    public interface IFilmeRepository : IRepositoryBase<Filme>
    {
        Task<IEnumerable<Filme>> GetFilmesAllAsync();
        Task<Filme> GetFilmesByIdAsync(int id);
        bool FilmeExists(int id);
    }
}
