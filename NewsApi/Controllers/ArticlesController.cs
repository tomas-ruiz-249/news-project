using Microsoft.AspNetCore.Mvc;
using News.Models;
using News.Services;

namespace News.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController(IArticleService articleService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Article>> Get() => await articleService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Article>> Get(string id)
    {
        var article = await articleService.GetAsync(id);

        if (article is null)
        {
            return NotFound();
        }

        return article;
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
        var article = await articleService.GetAsync(id);

        if (article is null)
        {
            return NotFound();
        }

        updatedArticle.Id = article.Id;

        await articleService.UpdateAsync(id, updatedArticle);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var article = await articleService.GetAsync(id);

        if (article is null)
        {
            return NotFound();
        }

        await articleService.RemoveAsync(id);

        return NoContent();
    }
}
