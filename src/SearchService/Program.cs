using SearchService;
using SearchService.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
await DbInitializer.InitDb(app);
app.MapControllers();

app.Run();

