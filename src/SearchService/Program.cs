using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

await DB.InitAsync("searchDb", MongoClientSettings
                                .FromConnectionString(
                                    builder.Configuration.GetConnectionString("MongoDbClient")));

await DB.Index<Item>()
    .Key(x=>x.Make, KeyType.Text)
    .Key(x=>x.Model, KeyType.Text)
    .Key(x=>x.Color, KeyType.Text)
    .CreateAsync();



app.Run();

