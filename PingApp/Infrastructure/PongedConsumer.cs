using MassTransit;
using ping_app.Domain;

namespace ping_app.Infrastructure;

public class PongedConsumer(ILogger<PongedConsumer> logger) : IConsumer<PongedEvent>
{
    public Task Consume(ConsumeContext<PongedEvent> context)
    {
        logger.LogInformation("Ponged: {Guid}", context.Message.Guid);
        return Task.CompletedTask;
    }
}