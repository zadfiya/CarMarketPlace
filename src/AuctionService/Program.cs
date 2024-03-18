using AuctionService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDBContext>(option=>{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.MapControllers();
try{
    DBInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
app.Run();

