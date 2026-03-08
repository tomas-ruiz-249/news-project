using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace News.WebCrawler;

class CrawlerWorker(IServiceScopeFactory scopeFactory, string[] urls) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CrawlListAsync(urls, stoppingToken);
    }

    private async Task CrawlListAsync(string[] urls, CancellationToken stoppingToken)
    {
        const int articlesPerSource = 15;
        foreach (var url in urls)
        {
            using var scope = scopeFactory.CreateScope();
            var crawler = scope.ServiceProvider.GetRequiredService<Crawler>();
            await crawler.CrawlAsync(new Uri(url), articlesPerSource, stoppingToken);
        }
    }
}
