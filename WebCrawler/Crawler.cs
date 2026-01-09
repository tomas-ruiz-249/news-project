using News.Models;

namespace News.WebCrawler;

class Crawler
{
    public Crawler()
    {
        _parser = new Parser();
        _urlQueue = [];
        _visitedUrls = [];
        _articleRepo = new ArticleRepositoryMongo();
    }

    public async Task Crawl(Uri startUrl, int articleCount)
    {
        _urlQueue.Enqueue(startUrl);
        var loadedArticleCount = 0;
        while (_urlQueue.Count > 0 && loadedArticleCount < articleCount)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"Queue: {_urlQueue.Count}, Visited: {_visitedUrls.Count}, Skipped: {_skippedCount}, Stored: {loadedArticleCount}"
            );
            Console.ForegroundColor = ConsoleColor.White;

            var currentUrl = _urlQueue.Dequeue();
            if (_visitedUrls.Contains(currentUrl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ALREADY VISITED {currentUrl}, SKIPPING...");
                Console.ForegroundColor = ConsoleColor.White;
                _skippedCount++;
                continue;
            }

            if (await _parser.Parse(currentUrl))
            {
                _visitedUrls.Add(currentUrl);

                var extractArticleTask = _parser.ExtractArticle();

                var urls = _parser.ExtractUrls();
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
                var article = await extractArticleTask;
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

                if (_articleRepo.StoreArticle(article))
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

    private readonly Parser _parser;
    private readonly Queue<Uri> _urlQueue;
    private readonly HashSet<Uri> _visitedUrls;
    private readonly IArticleRepository _articleRepo;
    private int _skippedCount = 0;
    private const int _queueLimit = 30;
}
