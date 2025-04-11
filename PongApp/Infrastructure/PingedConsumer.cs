using MassTransit;
using pong_app.Domain;

namespace pong_app.Infrastructure;

public class PingedConsumer(ILogger<PingedConsumer> logger) : IConsumer<PingedEvent>
{
    public Task Consume(ConsumeContext<PingedEvent> context)
    {
        logger.LogInformation("Pinged: {Guid}", context.Message.Guid);
        return Task.CompletedTask;
    }
}