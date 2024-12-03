using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Data.Repositories.Interfaces;
using AplicacaoWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoWeb.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsersAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public User GetUsersByIdAsync(int id)
        {
            return GetAllWhen(x => x.Id == id).FirstOrDefault();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
