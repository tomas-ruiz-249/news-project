using Microsoft.Extensions.Hosting;

namespace News.WebCrawler;

class CrawlerWorker(Crawler crawler, string[] urls) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CrawlListAsync(urls, stoppingToken);
    }

    private async Task CrawlListAsync(string[] urls, CancellationToken stoppingToken)
    {
        const int articlesPerSource = 10;
        foreach (var url in urls)
        {
            await crawler.CrawlAsync(new Uri(url), articlesPerSource, stoppingToken);
        }
    }
}
