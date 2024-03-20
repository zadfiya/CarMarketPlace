using SearchService;
using SearchService.DbInitializer;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();
await DbInitializer.InitDb(app);

app.Run();

