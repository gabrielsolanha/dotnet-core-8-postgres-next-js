using Microsoft.AspNetCore.Mvc;
using AplicacaoWeb.Responses;
using AplicacaoWeb.Models.Entities;
using AplicacaoWeb.Models.Dtos;
using AplicacaoWeb.Aplication.Interfaces;
using AplicacaoWeb.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IApp<CategoryDto> categoriesService;

    public CategoryController(IApp<CategoryDto> _categoriesService)
    {
        categoriesService = _categoriesService ?? throw new ArgumentNullException(nameof(_categoriesService));
    }

    [HttpPost("list")]
    public IActionResult ListCategories(PaginationDto<CategoryDto> filtro)
    {
        try
        {
            DataPaged<IEnumerable<CategoryDto>> dados =
            new DataPaged<IEnumerable<CategoryDto>>(filtro.ItemCount, categoriesService.List(filtro).ToList());
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
    public  ActionResult<Category> GetCategory(int id)
    {
        try
        {
            return Ok(categoriesService.Get(id));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao obter o category: " + ex.Message)
            );
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto category)
    {
        try
        {
            return Ok(await categoriesService.Add(category));
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao adicionar novo category: " + ex.Message)
            );
        }
    }

    [HttpPut]
    public async Task<ActionResult<CategoryDto>> PutCategory(CategoryDto category)
    {
        try
        {
            if(category.Id.HasValue) return Ok(await categoriesService.Update((int) category.Id, category));
            return BadRequest();
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao alterar o category: " + ex.Message)
            );
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await categoriesService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return new CustomErrorResult(
                500,
                new ErrorMessages("Ocorreu um erro ao deletar o category: " + ex.Message)
            );
        }
    }

}
