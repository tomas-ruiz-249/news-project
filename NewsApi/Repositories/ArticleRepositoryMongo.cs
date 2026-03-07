using MongoDB.Driver;
using News.Models;

namespace News.Repositories;

public class ArticleRepositoryMongo(IMongoCollection<Article> collection) : IArticleRepository
{
    public async Task CreateAsync(Article a)
    {
        await collection.InsertOneAsync(a);
    }

    public async Task<List<Article>> GetAsync()
    {
        return await collection.Find(_ => true).ToListAsync();
    }

    public async Task<Article?> GetAsync(string id)
    {
        return await collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var filter = Builders<Article>.Filter.Eq(a => a.Id, id);
        await collection.DeleteOneAsync(filter);
    }

    public async Task UpdateAsync(string id, Article newArticle)
    {
        var filter = Builders<Article>.Filter.Eq(a => a.Id, id);
        await collection.ReplaceOneAsync(filter, newArticle);
    }
}
