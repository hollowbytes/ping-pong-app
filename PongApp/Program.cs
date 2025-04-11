using MassTransit;
using pong_app.Configuration;
using pong_app.Domain;
using pong_app.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();

// Add services to the container.
builder.Services.AddMassTransit(bus =>
{
    bus.AddConsumer<PingedConsumer>();
    
    bus.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://guest:guest@rabbitmq:5672/");
        
        cfg.Message<PongedEvent>(x => x.SetEntityName("pinged-event"));

        cfg.ReceiveEndpoint("ponged-app", e =>
        {
            e.Bind("pinged-event");
            e.ConfigureConsumer<PingedConsumer>(context);
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

app.MapGet("/pong", async (IPublishEndpoint publishEndpoint) =>
    {
        await publishEndpoint.Publish(new PongedEvent(Guid.NewGuid()));
        return Results.Ok();
    })
    .WithName("Pong")
    .WithOpenApi();

app.Run();