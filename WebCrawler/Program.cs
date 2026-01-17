using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using News.WebCrawler;

string[] urls =
[
    "https://www.elespectador.com/",
    // "https://www.eltiempo.com/",
    // "https://www.elpais.com.co/",
];

const string apiUrl = "http://localhost:5039/api/articles";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<Crawler>();
builder.Services.AddSingleton<IParser, Parser>();
builder.Services.AddHttpClient<IArticleApiClient, ArticleApiClient>(
    (sp, client) =>
    {
        client.BaseAddress = new Uri(apiUrl);
    }
);
builder.Services.AddHostedService<CrawlerWorker>(sp =>
{
    var crawler = sp.GetRequiredService<Crawler>();
    return new CrawlerWorker(crawler, urls);
});

var host = builder.Build();
await host.RunAsync();
