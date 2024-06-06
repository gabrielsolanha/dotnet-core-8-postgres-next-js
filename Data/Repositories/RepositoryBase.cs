using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace AplicacaoWeb.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public IEnumerable<T> GetAllWhen(Expression<Func<T, bool>> predicado)
        {
            var query = _context.Set<T>().Where(predicado);

            var entityType = _context.Model.FindEntityType(typeof(T));
            var navigations = entityType.GetNavigations();

            foreach (var navigation in navigations)
            {
                query = query.Include(navigation.Name);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
