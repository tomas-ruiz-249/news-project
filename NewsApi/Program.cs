using Microsoft.Extensions.Options;
using MongoDB.Driver;
using News.Models;
using News.Repositories;
using News.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NewsDBSettings>(builder.Configuration.GetSection("NewsDatabase"));
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepositoryMongo>();
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NewsDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NewsDBSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<IMongoCollection<Article>>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NewsDBSettings>>().Value;
    var db = sp.GetRequiredService<IMongoDatabase>();
    return db.GetCollection<Article>(settings.ArticlesCollectionName);
});

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
