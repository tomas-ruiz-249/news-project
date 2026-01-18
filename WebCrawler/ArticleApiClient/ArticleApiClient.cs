using System.Net.Http.Json;

namespace News.WebCrawler;

class ArticleApiClient(HttpClient client) : IArticleApiClient
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.BackgroundColor = ConsoleColor.White;
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
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.BackgroundColor = ConsoleColor.White;
        }

        return html;
    }
}
