using AplicacaoWeb.Models.Dtos;

namespace AplicacaoWeb.Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserDto user);
        public string GetUserIdFromToken(string token);
    }
}
