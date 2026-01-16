namespace News.WebCrawler;

class ArticleApiClient : IArticleApiClient
{
    public ArticleApiClient(string host)
    {
        _client = new HttpClient();
        _host = host;
    }

    public Task<bool> StoreArticle(ScrapedArticle scrapedArticle)
    {
        throw new NotImplementedException();
    }

    private readonly HttpClient _client;
    private readonly string _host;
}
