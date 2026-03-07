using News.Models;
using News.Repositories;

namespace News.Services;

public class ArticleService(IArticleRepository repo) : IArticleService
{
    public async Task<List<Article>> GetAsync()
    {
        return await repo.GetAsync();
    }

    public async Task<Article?> GetAsync(string id)
    {
        return await repo.GetAsync(id);
    }

    public async Task CreateAsync(Article a)
    {
        await repo.CreateAsync(a);
    }

    public async Task UpdateAsync(string id, Article newArticle)
    {
        await repo.UpdateAsync(id, newArticle);
    }

    public async Task RemoveAsync(string id)
    {
        await repo.RemoveAsync(id);
    }
}
