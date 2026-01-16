using HtmlAgilityPack;
using SmartReader;

namespace News.WebCrawler;

class Parser : IParser
{
    public Parser()
    {
        _client = new HttpClient();
        _doc = new HtmlDocument();
        HtmlStr = "";
    }

    public async Task<bool> Parse(Uri url)
    {
        try
        {
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                HtmlStr = await response.Content.ReadAsStringAsync();
                _doc.LoadHtml(HtmlStr);
                _currentUrl = url;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Currently on " + url.AbsoluteUri);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }
        return true;
    }

    public List<Uri> ExtractUrls()
    {
        var list = new List<Uri>();
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
                    url = new Uri(_currentUrl!, hrefValue);
                }
                list.Add(url);
            }
            catch (System.Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        return list;
    }

    public async Task<ScrapedArticle> ExtractArticle()
    {
        var reader = new Reader(_currentUrl!.AbsoluteUri, HtmlStr);
        var extractedData = await reader.GetArticleAsync();

        return extractedData.IsReadable
            ? new ScrapedArticle
            {
                Title = extractedData.Title,
                Body = extractedData.TextContent,
                SiteName = extractedData.SiteName,
                Url = _currentUrl.AbsoluteUri,
                Author = extractedData.Author,
                PublicationDate = extractedData.PublicationDate,
            }
            : new ScrapedArticle();
    }

    private HttpClient _client { get; init; }
    private HtmlDocument _doc { get; }
    public Uri? _currentUrl { get; private set; }
    public string HtmlStr { get; private set; }
}
