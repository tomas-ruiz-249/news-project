using Microsoft.AspNetCore.Mvc;
using News.Models;
using News.Services;

namespace News.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticlesController(ArticleService articleService) => _articleService = articleService;

    [HttpGet]
    public async Task<List<Article>> Get() => await _articleService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Article>> Get(string id)
    {
        var book = await _articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Article newArticle)
    {
        await _articleService.CreateAsync(newArticle);

        return CreatedAtAction(nameof(Get), new { id = newArticle.Id }, newArticle);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Article updatedArticle)
    {
        var book = await _articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedArticle.Id = book.Id;

        await _articleService.UpdateAsync(id, updatedArticle);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _articleService.RemoveAsync(id);

        return NoContent();
    }
}
