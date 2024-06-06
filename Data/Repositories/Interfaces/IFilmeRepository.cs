using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IFilmeRepository : IRepositoryBase<Filme>
    {
        Task<IEnumerable<Filme>> GetFilmesAllAsync();
        Task<Filme> GetFilmesByIdAsync(int id);
        bool FilmeExists(int id);
    }
}
