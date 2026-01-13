using Microsoft.AspNetCore.Mvc;
using News.Models;
using News.Services;

namespace News.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController(IArticleService articleService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Article>> Get() => await articleService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Article>> Get(string id)
    {
        var book = await articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Article newArticle)
    {
        await articleService.CreateAsync(newArticle);

        return CreatedAtAction(nameof(Get), new { id = newArticle.Id }, newArticle);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Article updatedArticle)
    {
        var book = await articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedArticle.Id = book.Id;

        await articleService.UpdateAsync(id, updatedArticle);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await articleService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await articleService.RemoveAsync(id);

        return NoContent();
    }
}
