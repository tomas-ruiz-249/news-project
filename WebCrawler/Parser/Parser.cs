using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SmartReader;

namespace News.WebCrawler;

class Parser(ILogger<Parser> logger) : IParser
{
    public async Task<ScrapedArticle> ParseAsync(string html, Uri current)
    {
        var reader = new Reader(current.AbsoluteUri, html);
        var extractedData = await reader.GetArticleAsync();

        return extractedData.IsReadable
            ? new ScrapedArticle
            {
                Title = extractedData.Title,
                Body = extractedData.TextContent,
                SiteName = extractedData.SiteName,
                Url = extractedData.Uri.AbsoluteUri,
                Author = extractedData.Author,
                PublicationDate = extractedData.PublicationDate,
            }
            : new ScrapedArticle();
    }

    public List<Uri> ExtractUrls(string html, Uri current)
    {
        var list = new List<Uri>();
        var _doc = new HtmlDocument();
        _doc.LoadHtml(html);
        var urlNodes = _doc.DocumentNode.SelectNodes("//a[@href]");
        if (urlNodes == null)
            return list;

        foreach (var node in urlNodes)
        {
            var hrefValue = node.Attributes["href"]?.Value;
            if (hrefValue == null)
                continue;

            try
            {
                Uri url;
                if (Uri.IsWellFormedUriString(hrefValue, UriKind.Absolute))
                {
                    url = new Uri(hrefValue);
                }
                else
                {
                    url = new Uri(current!, hrefValue);
                }
                list.Add(url);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        return list;
    }
}
