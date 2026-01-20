using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace News.WebCrawler;

class ArticleApiClient(HttpClient client, ILogger<ArticleApiClient> logger) : IArticleApiClient
{
    public async Task<bool> StoreArticleAsync(ScrapedArticle scrapedArticle)
    {
        try
        {
            var response = await client.PostAsJsonAsync<ScrapedArticle>("", scrapedArticle);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return false;
        }
    }

    public async Task<string> GetHtmlFromUrl(Uri url)
    {
        var html = "";
        try
        {
            html = await client.GetStringAsync(url);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return html;
    }
}
