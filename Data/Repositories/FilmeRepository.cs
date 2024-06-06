using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Data.Repository
{
    public class FilmeRepository : RepositoryBase<Filme>, IFilmeRepository
    {
        private readonly AppDbContext _context;

        public FilmeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Filme>> GetFilmesAllAsync()
        {
            return await _context.Filmes
                .ToListAsync();
        }

        public async Task<Filme> GetFilmesByIdAsync(int id)
        {
            return await _context.Filmes
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.Id == id);
        }
        public new void Update(Filme entity)
        {
            var existingEntity = _context.Filmes.Find(entity.Id);
            if (existingEntity == null)
            {
                _context.Filmes.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }
    }
}
