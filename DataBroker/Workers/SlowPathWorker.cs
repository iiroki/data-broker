using DataBroker.Models;
using MassTransit;

namespace DataBroker.Workers;

public class SlowPathWorker(
    ISendEndpointProvider endpointProvider,
    IHostApplicationLifetime lifetime,
    ILogger<SlowPathWorker> logger
)
    : DataGeneratorWorker(
        DataBrokerPath.Slow,
        ["SLOW-1", "SLOW-2", "SLOW-3"],
        endpointProvider,
        TimeSpan.FromMilliseconds(10),
        lifetime,
        logger
    );
