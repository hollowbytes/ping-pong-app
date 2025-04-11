using MassTransit;

namespace pong_app.Domain;

[MessageUrn("pinged-event")]
public record PingedEvent(Guid Guid);

[MessageUrn("ponged-event")]
public record PongedEvent(Guid Guid);