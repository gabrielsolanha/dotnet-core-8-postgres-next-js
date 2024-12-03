using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Data.Repository
{
    public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
    {
        private readonly AppDbContext _context;

        public BannerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Banner>> GetBannersAllAsync()
        {
            return await _context.Banners
                .ToListAsync();
        }

        public Banner? GetBannersById(int id)
        {
            return GetAllWhen(x => x.Id == id).FirstOrDefault();
        }

        public bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
        public new void Update(Banner entity)
        {
            var existingEntity = _context.Banners.Find(entity.Id);
            if (existingEntity == null)
            {
                _context.Banners.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }
    }
}
