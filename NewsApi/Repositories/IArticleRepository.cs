using News.Models;

namespace News.Repositories;

public interface IArticleRepository
{
    public Task<List<Article>> GetAsync();
    public Task<Article?> GetAsync(string id);
    public Task CreateAsync(Article a);
    public Task UpdateAsync(string id, Article newArticle);
    public Task RemoveAsync(string id);
}
