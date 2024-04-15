using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplicacaoWeb.Models;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Services;
using AplicacaoWeb.Data.Context;

[Route("API/[controller]")]
[ApiController]
public class FilmesController : ControllerBase
{
    private readonly IService<Filme> filmesService;

    public FilmesController(IService<Filme> _filmesService)
    {
        filmesService = _filmesService ?? throw new ArgumentNullException(nameof(_filmesService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
    {
        try
        {
            return Ok(await filmesService.List());
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
            return Ok(await filmesService.Get(id));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter o filme: " + ex.Message)
            );
        }
    }

    [HttpPost]
    public async Task<ActionResult<Filme>> PostFilme(Filme filme)
    {
        try
        {
            return Ok(await filmesService.Add(filme));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao adicionar novo filme: " + ex.Message)
            );
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFilme(int id, Filme filme)
    {
        try
        {
            if (id != filme.Id)
            {
                return BadRequest();
            }
            await filmesService.Update(id, filme);
            return Ok();
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao alterar o filme: " + ex.Message)
            );
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilme(int id)
    {
        try
        {
            await filmesService.Delete(id);
            return Ok();
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
