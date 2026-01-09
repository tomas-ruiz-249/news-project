namespace News.Models;

public class NewsDBSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ArticlesCollectionName { get; set; } = null!;
}
