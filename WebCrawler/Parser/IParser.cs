namespace News.WebCrawler;

interface IParser
{
    Task<ScrapedArticle> ParseAsync(string html, Uri current);

    List<Uri> ExtractUrls(string html, Uri current);
}
