using AuctionService;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDBContext>(option=>{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x=>{
    x.AddEntityFrameworkOutbox<AuctionDBContext>(o=>{
        // The delay between queries once messages are no longer available. When a query returns messages, subsequent queries are performed until no messages are returned after which the QueryDelay is used.
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.AddActivitiesFromNamespaceContaining<AuctionCreatedFaultyConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction",false));

    x.UsingRabbitMq( (context,cfg) =>{
            //Configure the endpoints for all defined consumer, saga, and activity types using an optional endpoint name formatter. 
            //If no endpoint name formatter is specified and an IEndpointNameFormatter is registered in the container, it is resolved from the container. 
            //Otherwise, the DefaultEndpointNameFormatter is used.
            cfg.ConfigureEndpoints(context);
        }
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>{
                    options.Authority = builder.Configuration["Identity:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.NameClaimType = "username";

                });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
try{
    DBInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
app.Run();

