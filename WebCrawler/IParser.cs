namespace News.WebCrawler;

interface IParser
{
    Task<bool> Parse(Uri url);

    List<Uri> ExtractUrls();

    Task<ScrapedArticle> ExtractArticle();
}
