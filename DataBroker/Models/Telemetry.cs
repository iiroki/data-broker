namespace DataBroker.Models;

public record Telemetry
{
    public required string Key { get; init; }

    public required DateTime Timestamp { get; init; }

    public required double Value { get; init; }
}
