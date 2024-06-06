using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Data.Repository
{
    public class ScreenRepository : RepositoryBase<Screen>, IScreenRepository
    {
        private readonly AppDbContext _context;

        public ScreenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Screen>> GetScreensAllAsync()
        {
            return await _context.Screens
                .ToListAsync();
        }

        public async Task<Screen> GetScreensByIdAsync(int id)
        {
            return await _context.Screens
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public bool ScreenExists(int id)
        {
            return _context.Screens.Any(e => e.Id == id);
        }
    }
}
