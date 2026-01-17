namespace News.WebCrawler;

interface IArticleApiClient
{
    Task<bool> StoreArticleAsync(ScrapedArticle scrapedArticle);
    Task<string> GetHtmlFromUrl(Uri url);
}
