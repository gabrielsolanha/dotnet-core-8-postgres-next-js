using AplicacaoWeb.Models.Dtos.Requests;
using AplicacaoWeb.Models.Dtos.Responses;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Enums;

namespace AplicacaoWeb.Service.Interfaces
{
    public interface IAuthService
    {
        User? GetUser(string userName);
        string AccessLevelToString(AccessLevel accessLevel);
        LoginResponse? GetLoginResponse(User user, LoginRequest loginRequest);
        List<ViewResponse> GetListView(User user);
    }
}
