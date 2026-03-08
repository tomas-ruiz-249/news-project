using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using News.WebCrawler;

string[] urls = ["https://www.elespectador.com/"];

const string apiUrl = "http://localhost:5039/api/articles";

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<Crawler>();
builder.Services.AddSingleton<IParser, Parser>();
builder.Services.AddHttpClient<IArticleApiClient, ArticleApiClient>(
    (sp, client) =>
    {
        client.BaseAddress = new Uri(apiUrl);
    }
);
builder.Services.AddHostedService<CrawlerWorker>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new CrawlerWorker(scopeFactory, urls);
});

var host = builder.Build();
await host.RunAsync();
await host.StopAsync();
