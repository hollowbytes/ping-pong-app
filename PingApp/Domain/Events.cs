using MassTransit;

namespace ping_app.Domain;

[MessageUrn("pinged-event")]
public record PingedEvent(Guid Guid);

[MessageUrn("ponged-event")]
public record PongedEvent(Guid Guid);