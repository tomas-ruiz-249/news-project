using News.WebCrawler;

var crawler = new Crawler();

string[] urls =
[
    "https://www.elespectador.com/",
    // "https://www.eltiempo.com/",
    // "https://www.elpais.com.co/",
];

const int articlesPerSource = 10;
foreach (var url in urls)
{
    await crawler.Crawl(new Uri(url), articlesPerSource);
}
