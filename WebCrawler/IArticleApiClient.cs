namespace News.WebCrawler;

interface IArticleApiClient
{
    Task<bool> StoreArticle(ScrapedArticle scrapedArticle);
}
