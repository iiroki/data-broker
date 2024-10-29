namespace DataBroker.Helpers;

public static class EndpointHelper
{
    public static string ToPathName(string path) => $"path__{path}";

    public static Uri ToPathQueueUri(string path) => new($"queue:{ToPathName(path)}");
}
