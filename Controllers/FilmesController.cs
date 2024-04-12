// Controllers/FilmesController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplicacaoWeb.Data;
using AplicacaoWeb.Models;


[Route("API/[controller]")]
[ApiController]
public class FilmesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FilmesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Filme>>> GetArticles()
    {
        return await _context.Filmes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Filme>> GetArticle(int id)
    {
        var article = await _context.Filmes.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (article == null)
        {
            return NotFound();
        }

        return article;
    }

    [HttpPost]
    public async Task<ActionResult<Filme>> PostArticle(Filme article)
    {
        article.CreatedAt = DateTime.UtcNow; // Converter para UTC
        _context.Filmes.Add(article);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetArticle", new { id = article.Id }, article);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArticle(int id, Filme article)
    {
        if (id != article.Id)
        {
            return BadRequest();
        }

        _context.Entry(article).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArticleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        Filme article = await _context.Filmes.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (article == null)
        {
            return NotFound();
        }

        _context.Filmes.Remove(article);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ArticleExists(int id)
    {
        return _context.Filmes.Any(e => e.Id == id);
    }
}