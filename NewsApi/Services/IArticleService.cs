using News.Models;

namespace News.Services;

public interface IArticleService
{
    Task<List<Article>> GetAsync();
    Task<Article?> GetAsync(string id);
    Task<bool> CreateAsync(Article a);
    Task UpdateAsync(string id, Article a);
    Task RemoveAsync(string id);
}
