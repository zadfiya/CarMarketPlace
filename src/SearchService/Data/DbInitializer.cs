using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService.DbInitializer;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("searchDb", MongoClientSettings
                                .FromConnectionString(
                                    app.Configuration.GetConnectionString("MongoDbClient")));

        await DB.Index<Item>()
            .Key(x=>x.Make, KeyType.Text)
            .Key(x=>x.Model, KeyType.Text)
            .Key(x=>x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();
        if(count==0)
        {
            Console.WriteLine("Seeding Data");
            await SeedData();
            return ;
        }
    }

    public static async Task SeedData()
    {
        var data = await File.ReadAllTextAsync("Data/Auction.json");
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};
        var auctions = JsonSerializer.Deserialize<List<Item>>(data,options);

        await DB.SaveAsync<Item>(auctions);
    }
}
