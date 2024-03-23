using AuctionService;
using Microsoft.EntityFrameworkCore;
using MassTransit;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDBContext>(option=>{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=>{
    x.UsingRabbitMq( (context,cfg) =>{
            //Configure the endpoints for all defined consumer, saga, and activity types using an optional endpoint name formatter. 
            //If no endpoint name formatter is specified and an IEndpointNameFormatter is registered in the container, it is resolved from the container. 
            //Otherwise, the DefaultEndpointNameFormatter is used.
            cfg.ConfigureEndpoints(context);
        }
    );
});
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

