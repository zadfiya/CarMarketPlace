using MassTransit;
using SearchService;
using SearchService.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=>{

    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>(); //Adds all consumers from the assembly containing the specified type that are in the same (or deeper) namespace.

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false)); // Set the default endpoint name formatter used for endpoint names.

    x.UsingRabbitMq( (context,cfg) =>{
            //Configure the endpoints for all defined consumer, saga, and activity types using an optional endpoint name formatter. 
            //If no endpoint name formatter is specified and an IEndpointNameFormatter is registered in the container, it is resolved from the container. 
            //Otherwise, the DefaultEndpointNameFormatter is used.
            cfg.ConfigureEndpoints(context);
        }
    );
});

var app = builder.Build();
await DbInitializer.InitDb(app);
app.MapControllers();

app.Run();

