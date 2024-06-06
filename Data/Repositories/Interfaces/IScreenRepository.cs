using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IScreenRepository : IRepositoryBase<Screen>
    {
        Task<IEnumerable<Screen>> GetScreensAllAsync();
        Task<Screen> GetScreensByIdAsync(int id);
        bool ScreenExists(int id);
    }
}
