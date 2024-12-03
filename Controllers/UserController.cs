using Microsoft.AspNetCore.Mvc;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using AplicacaoWeb.Service.Interfaces;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUsersApp usersService;
    private readonly IAuthService authService;

    public UserController(IUsersApp _usersService, IAuthService _authService)
    {
        authService = _authService ?? throw new ArgumentNullException(nameof(_authService));
        usersService = _usersService ?? throw new ArgumentNullException(nameof(_usersService));
    }

    [HttpGet("public/{id}")]
    public async Task<ActionResult<Filme>> GetUserPublic(int id)
    {
        try
        {
            return Ok(await usersService.GetUserPublicInfo(id));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter o user: " + ex.Message)
            );
        }
    }
    [Authorize]
    [HttpPost("list")]
    public async Task<IActionResult> ListUsers(PaginationDto<UserDto> filtro)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = await authService.VerifyTokenAccess(token, "User", "View");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            DataPaged<IEnumerable<UserDto>> dados;
            if (access.GetType() == typeof(UserDto))
            {
                var us = new List<UserDto>
                {
                    (UserDto)access
                };
                dados = new DataPaged<IEnumerable<UserDto>>(filtro.ItemCount, usersService.List(filtro).ToList());
                filtro.ItemCount = 1;
            }
            dados = new DataPaged<IEnumerable<UserDto>>(filtro.ItemCount, usersService.List(filtro).ToList());
            dados.Size = filtro.ItemCount;
            return Ok(dados);
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter a lista: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public  async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "User", "View");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            return Ok(usersService.Get(id));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter o user: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDto user)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "User", "Create");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            return Ok(await usersService.Add(user, access.ToString()));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao adicionar novo user: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<UserDto>> PutUser(UserDto user)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "User", "Update");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            if (user.Id.HasValue) return Ok(await usersService.Update((int) user.Id, user, access.ToString()));
            return BadRequest();
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao alterar o user: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "User", "Delete");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            await usersService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao deletar o user: " + ex.Message)
            );
        }
    }

}
