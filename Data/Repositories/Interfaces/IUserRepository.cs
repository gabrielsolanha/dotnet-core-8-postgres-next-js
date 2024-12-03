using AplicacaoWeb.Models.Entities;

namespace AplicacaoWeb.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetUsersAllAsync();
        User GetUsersByIdAsync(int id);
        bool UserExists(int id);
    }
}
