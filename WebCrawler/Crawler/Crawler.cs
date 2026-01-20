using Microsoft.Extensions.Logging;

namespace News.WebCrawler;

class Crawler(IArticleApiClient client, IParser parser, ILogger<Crawler> logger)
{
    private readonly Queue<Uri> _urlQueue = [];
    private readonly HashSet<Uri> _visitedUrls = [];

    public async Task CrawlAsync(Uri startUrl, int articleCount, CancellationToken stoppingToken)
    {
        _urlQueue.Enqueue(startUrl);

        var loadedArticleCount = 0;
        var skippedCount = 0;

        while (
            _urlQueue.Count > 0
            && loadedArticleCount < articleCount
            && !stoppingToken.IsCancellationRequested
        )
        {
            logger.LogInformation(
                $"Queue: {_urlQueue.Count}, Visited: {_visitedUrls.Count}, Skipped: {skippedCount}, Stored: {loadedArticleCount}"
            );

            var currentUrl = _urlQueue.Dequeue();
            if (_visitedUrls.Contains(currentUrl))
            {
                logger.LogError("ALREADY VISITED {currentUrl}, SKIPPING...", currentUrl);
                skippedCount++;
                continue;
            }

            var html = await client.GetHtmlFromUrl(currentUrl);
            if (!string.IsNullOrEmpty(html))
            {
                _visitedUrls.Add(currentUrl);

                var parseArticleTask = parser.ParseAsync(html, currentUrl);

                var urls = parser.ExtractUrls(html, currentUrl);
                foreach (var extractedUrl in urls)
                {
                    if (
                        _visitedUrls.Contains(extractedUrl)
                        || (
                            !currentUrl.Host.Contains(extractedUrl.Host)
                            && !extractedUrl.Host.Contains(currentUrl.Host)
                        )
                    )
                    {
                        continue;
                    }
                    _urlQueue.Enqueue(extractedUrl);
                }

                //extract article
                var article = await parseArticleTask;
                logger.LogInformation("{article}", article);

                //store article
                if (article.Body == null || article.Body.Length < 300)
                {
                    logger.LogError("Article body null or too short");
                    continue;
                }

                if (await client.StoreArticleAsync(article))
                {
                    logger.LogInformation("Stored Article Properly");
                    loadedArticleCount++;
                }
                else
                {
                    logger.LogError("Error storing article");
                }
            }
            else
            {
                logger.LogError(
                    "Could not obtain HTML for the given url: {AbsoluteUri}",
                    startUrl.AbsoluteUri
                );
            }
        }
        _urlQueue.Clear();
        _visitedUrls.Clear();
        logger.LogInformation("Crawler stored {loadedArticleCount} Articles.", loadedArticleCount);
    }
}
