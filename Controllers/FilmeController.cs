using Microsoft.AspNetCore.Mvc;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Aplication;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Dtos.Filme;
using AplicacaoWeb.Models.Dtos;
using Amazon.S3.Model;

[Route("API/[controller]")]
[ApiController]
public class FilmeController : ControllerBase
{
    private readonly IFilmesApp filmesService;

    public FilmeController(IFilmesApp _filmesService)
    {
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
    public async Task<ActionResult<FilmeDto>> PostFilme(FilmeWithArquiveDto filme)
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

    [HttpPut]
    public async Task<ActionResult<FilmeDto>> PutFilme(FilmeWithArquiveDto filme)
    {
        try
        {
            if(filme.Id.HasValue) return Ok(await filmesService.Update((int) filme.Id, filme));
            return BadRequest();
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
