using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IUsersApp : IApp<UserDto>
    {
        new Task<UserDto> Add(UserDto obj);
        new Task<UserDto> Update(int id, UserDto obj);
    }
}
