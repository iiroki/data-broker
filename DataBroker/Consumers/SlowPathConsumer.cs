using DataBroker.Models;
using MassTransit;

namespace DataBroker.Consumers;

public class SlowPathConsumer(ILogger<SlowPathConsumer> logger) : IConsumer<Batch<Telemetry>>
{
    private readonly ILogger _logger = logger;

    public Task Consume(ConsumeContext<Batch<Telemetry>> ctx)
    {
        _logger.LogInformation(
            "Slow - Received {C} message(s) from {F} to {T}",
            ctx.Message.Length,
            ctx.Message.FirstMessageReceived,
            ctx.Message.LastMessageReceived
        );

        return Task.CompletedTask;
    }
}
