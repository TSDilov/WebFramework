using WebFramework.Core;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var server = new HttpServer(8080);
        await server.StartAsync();
    }
}