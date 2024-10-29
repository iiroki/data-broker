using DataBroker.Models;
using MassTransit;

namespace DataBroker.Consumers;

public class FastPathConsumer(ILogger<FastPathConsumer> logger) : IConsumer<Telemetry>
{
    private readonly ILogger _logger = logger;

    public Task Consume(ConsumeContext<Telemetry> ctx)
    {
        _logger.LogInformation("Fast - Received: {M}", ctx.Message);
        return Task.CompletedTask;
    }
}
