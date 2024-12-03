using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IFilmeRepository : IRepositoryBase<Filme>
    {
        Task<IEnumerable<Filme>> GetFilmesAllAsync();
        Filme? GetFilmesById(int id);
        bool FilmeExists(int id);
    }
}
