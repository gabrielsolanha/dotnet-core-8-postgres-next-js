using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Aplication.Interfaces
{
    public interface IUsersApp : IApp<UserDto>
    {
        new Task<UserDto> GetUserPublicInfo(int id);
    }
}
