using MassTransit;
using ping_app.Configuration;
using ping_app.Domain;
using ping_app.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();

// Add services to the container.
builder.Services.AddMassTransit(bus =>
{
    bus.AddConsumer<PongedConsumer>();
    
    bus.UsingRabbitMq((context, cfg) =>
    {        
        cfg.Host("amqps://guest:guest@rabbitmq:5672/");
        
        cfg.Message<PingedEvent>(x => x.SetEntityName("pinged-event"));

        cfg.ReceiveEndpoint("pinged-app", e =>
        {
            e.Bind("ponged-event");
            e.ConfigureConsumer<PongedConsumer>(context);
        });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/ping", async (IPublishEndpoint publishEndpoint) =>
    {
        await publishEndpoint.Publish(new PingedEvent(Guid.NewGuid()));
        return Results.Ok();
    })
    .WithName("Ping")
    .WithOpenApi();

app.Run();