using AplicacaoWeb.Models.Dtos.Requests;
using AplicacaoWeb.Models.Dtos.Responses;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService _authService)
    {
        authService = _authService ?? throw new ArgumentNullException(nameof(_authService));
    }
    [HttpPost]
    public IActionResult Auth(LoginRequest loginRequest)
    {

        try
        {
            var userEntity = authService.GetUser(loginRequest.Username);
            if (userEntity == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuario não encontrado! ")
                );
            }
            var loginResponse = authService.GetLoginResponse(userEntity, loginRequest);
            if (loginResponse == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Senha inválida! ")
                );
            }
            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter informções: " + ex.Message)
            );
        }


    }
}
