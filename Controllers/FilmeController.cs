using Microsoft.AspNetCore.Mvc;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using AplicacaoWeb.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;

[Route("api/v1/[controller]")]
[ApiController]
public class FilmeController : ControllerBase
{
    private readonly IFilmesApp filmesService;
    private readonly IAuthService authService;

    public FilmeController(IFilmesApp _filmesService, IAuthService _authService)
    {
        authService = _authService ?? throw new ArgumentNullException(nameof(_authService));
        filmesService = _filmesService ?? throw new ArgumentNullException(nameof(_filmesService));
    }

    [HttpPost("list")]
    public IActionResult ListFilmes(PaginationDto<FilmeDto> filtro)
    {
        try
        {
            DataPaged<IEnumerable<FilmeDto>> dados =
            new DataPaged<IEnumerable<FilmeDto>>(filtro.ItemCount, filmesService.List(filtro).ToList());
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Filme>> GetFilme(int id)
    {
        try
        {
            return Ok(filmesService.Get(id));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter o filme: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FilmeDto>> PostFilme(FilmeWithArquiveDto filme)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Filme", "Create");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            return Ok(await filmesService.Add(filme, access.ToString()));
        }
        catch (SecurityTokenExpiredException)
        {
            return new CustomErrorResult(
                400,
                new ErrorMessages("O token está expirado. ")
            );
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao adicionar novo filme: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<FilmeDto>> PutFilme(FilmeWithArquiveDto filme)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Filme", "Update");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            if (filme.Id.HasValue) return Ok(await filmesService.Update((int) filme.Id, filme, access.ToString()));
            return BadRequest();
        }
        catch (SecurityTokenExpiredException)
        {
            return new CustomErrorResult(
                400,
                new ErrorMessages("O token está expirado. ")
            );
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao alterar o filme: " + ex.Message)
            );
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilme(int id)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Filme", "Delete");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            await filmesService.Delete(id);
            return Ok();
        }
        catch (SecurityTokenExpiredException)
        {
            return new CustomErrorResult(
                400,
                new ErrorMessages("O token está expirado. ")
            );
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao deletar o filme: " + ex.Message)
            );
        }
    }

}
