using DataBroker.Helpers;
using DataBroker.Models;
using MassTransit;

namespace DataBroker.Workers;

public abstract class DataGeneratorWorker(
    string path,
    IEnumerable<string> keys,
    ISendEndpointProvider endpointProvider,
    TimeSpan timeout,
    IHostApplicationLifetime lifetime,
    ILogger logger
) : BackgroundService
{
    private readonly string _path = path;
    private readonly List<string> _keys = keys.ToList();
    private readonly ISendEndpointProvider _endpointProvider = endpointProvider;
    private readonly TimeSpan _timeout = timeout;
    private readonly ILogger _logger = logger;

    private readonly CancellationToken _ct = lifetime.ApplicationStopping;

    protected override async Task ExecuteAsync(CancellationToken _)
    {
        _logger.LogInformation("Starting...");
        var uri = EndpointHelper.ToPathQueueUri(_path);
        var endpoint = await _endpointProvider.GetSendEndpoint(uri);

        var random = new Random();
        while (!_ct.IsCancellationRequested)
        {
            try
            {
                var tasks = new Task[_keys.Count];
                for (var i = 0; i < _keys.Count; i++)
                {
                    var data = new Telemetry
                    {
                        Key = _keys[i],
                        Timestamp = DateTime.UtcNow,
                        Value = Math.Round(random.NextDouble() * 100, 2),
                    };

                    tasks[i] = endpoint.Send(data, _ct);
                }

                await Task.WhenAll(tasks);
                if (_timeout.Ticks > 0)
                {
                    await Task.Delay(_timeout, _ct);
                }
            }
            catch (OperationCanceledException)
            {
                // NOP
            }
        }

        _logger.LogInformation("Completed");
    }
}
