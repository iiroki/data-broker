using DataBroker.Models;
using MassTransit;

namespace DataBroker.Workers;

public class FastPathWorker(
    ISendEndpointProvider endpointProvider,
    IHostApplicationLifetime lifetime,
    ILogger<FastPathWorker> logger
)
    : DataGeneratorWorker(
        DataBrokerPath.Fast,
        ["FAST-1", "FAST-2"],
        endpointProvider,
        TimeSpan.FromSeconds(5),
        lifetime,
        logger
    );
