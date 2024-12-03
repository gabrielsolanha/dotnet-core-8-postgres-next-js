using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IBannerRepository : IRepositoryBase<Banner>
    {
        Task<IEnumerable<Banner>> GetBannersAllAsync();
        Banner? GetBannersById(int id);
        bool BannerExists(int id);
    }
}
