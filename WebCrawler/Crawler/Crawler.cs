namespace News.WebCrawler;

class Crawler(IArticleApiClient client, IParser parser)
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"Queue: {_urlQueue.Count}, Visited: {_visitedUrls.Count}, Skipped: {skippedCount}, Stored: {loadedArticleCount}"
            );
            Console.ForegroundColor = ConsoleColor.White;

            var currentUrl = _urlQueue.Dequeue();
            if (_visitedUrls.Contains(currentUrl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ALREADY VISITED {currentUrl}, SKIPPING...");
                Console.ForegroundColor = ConsoleColor.White;
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
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(article);
                Console.ForegroundColor = ConsoleColor.White;

                //store article
                if (article.Body == null || article.Body.Length < 300)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Article body null or too short");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                if (await client.StoreArticleAsync(article))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Stored Article Properly");
                    Console.ForegroundColor = ConsoleColor.White;
                    loadedArticleCount++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error storing article");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Could not obtain HTML for the given url: " + startUrl.AbsoluteUri
                );
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        _urlQueue.Clear();
        _visitedUrls.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Crawler stored {loadedArticleCount} Articles.");
        Console.ForegroundColor = ConsoleColor.White;
    }
}
