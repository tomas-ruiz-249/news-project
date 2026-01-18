using System.Text.Json.Serialization;

namespace News.WebCrawler;

class ScrapedArticle
{
    public ScrapedArticle() { }

    public ScrapedArticle(string? title, string? body, string? siteName, string url, string? author)
    {
        Title = title;
        Body = body;
        SiteName = siteName;
        Url = url;
        Author = author;
    }

    public string? Title { get; set; } = null;
    public string? Body { get; set; } = null;
    public string? SiteName { get; set; } = null;

    [JsonRequired]
    public string Url { get; set; }

    public string? Author { get; set; } = null;
    public DateTime? PublicationDate { get; set; } = null;
    public DateTime ExtractionDate { get; set; }

    public override string ToString()
    {
        return $"""
            Author: {Author}
            Title: {Title}
            Body: {Body}
            SiteName: {SiteName}
            Url: {Url}
            PublicationDate: {PublicationDate}
            ExtractionDate: {ExtractionDate}
            """;
    }
}
