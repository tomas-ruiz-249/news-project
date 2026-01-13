using MongoDB.Driver;
using News.Models;

namespace News.Services;

public class ArticleService : IArticleService
{
    public ArticleService(IMongoCollection<Article> collection) => _collection = collection;

    public async Task<List<Article>> GetAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Article?> GetAsync(string id)
    {
        return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Article a)
    {
        await _collection.InsertOneAsync(a);
    }

    public async Task UpdateAsync(string id, Article newArticle)
    {
        var filter = Builders<Article>.Filter.Eq(a => a.Id, id);
        await _collection.ReplaceOneAsync(filter, newArticle);
    }

    public async Task RemoveAsync(string id)
    {
        var filter = Builders<Article>.Filter.Eq(a => a.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    private readonly IMongoCollection<Article> _collection;
}
