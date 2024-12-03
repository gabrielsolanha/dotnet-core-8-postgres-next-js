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
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IApp<CategoryDto> categoriesService;
    private readonly IAuthService authService;

    public CategoryController(IApp<CategoryDto> _categoriesService, IAuthService _authService)
    {
        authService = _authService ?? throw new ArgumentNullException(nameof(_authService));
        categoriesService = _categoriesService ?? throw new ArgumentNullException(nameof(_categoriesService));
    }

    [HttpPost("list")]
    public async Task<IActionResult> ListCategories(PaginationDto<CategoryDto> filtro)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = await authService.VerifyTokenAccess(token, "Category", "View");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
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
    public  async Task<ActionResult<Category>> GetCategory(int id)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Category", "View");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
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
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Category", "Create");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            return Ok(await categoriesService.Add(category, access.ToString()));
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
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Category", "Update");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
            if (category.Id.HasValue) return Ok(await categoriesService.Update((int) category.Id, category, access.ToString()));
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
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var access = authService.VerifyTokenAccess(token, "Category", "Delete");
            if (access == null)
            {
                return new CustomErrorResult(
                    400,
                    new ErrorMessages("Usuário não tem este acesso.")
                );
            }
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
