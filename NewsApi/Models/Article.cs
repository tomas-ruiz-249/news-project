using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace News.Models;

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonIgnoreIfNull]
    public string? Author { get; set; }

    [BsonIgnoreIfNull]
    public string? Title { get; set; }

    [BsonIgnoreIfNull]
    public string? Body { get; set; }

    [BsonIgnoreIfNull]
    public string SiteName { get; set; } = null!;

    public string? Url { get; set; }

    [BsonIgnoreIfNull]
    public DateTime? PublicationDate { get; set; }

    public DateTime ExtractionDate { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return $"""
            Author: {Author}
            Title: {Title}
            Body: {Body}
            SiteName: {SiteName}
            Url: {Url}
            PublicationDate: {PublicationDate}
            ExtractionDate: {ExtractionDate}
            """;
    }
}
