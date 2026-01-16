using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using News.WebCrawler;

var builder = Host.CreateApplicationBuilder();

string[] urls =
[
    "https://www.elespectador.com/",
    // "https://www.eltiempo.com/",
    // "https://www.elpais.com.co/",
];

var crawler = new Crawler(new ArticleApiClient("https://localhost:5039/"), new Parser());

const int articlesPerSource = 10;
foreach (var url in urls)
{
    await crawler.CrawlAsync(new Uri(url), articlesPerSource);
}
