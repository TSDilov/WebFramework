namespace WebFramework.Core
{
    using System.Net.Sockets;
    using System.Net;
    using System.Text;
    using WebFramework.Routing;

    public class HttpServer
    {
        private readonly TcpListener _listener;
        private readonly Router _router = new Router();

        public HttpServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);

            _router.RegisterRoute("/", () => "HTTP/1.1 200 OK\r\n\r\nWelcome to the Home Page!");
            _router.RegisterRoute("/about", () => "HTTP/1.1 200 OK\r\n\r\nAbout this Web Framework");
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Server started on port 8080...");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var buffer = new byte[1024 * 4];
            var stream = client.GetStream();
            var length = await stream.ReadAsync(buffer, 0, buffer.Length);
            var request = Encoding.UTF8.GetString(buffer, 0, length);

            var requestLine = request.Split("\r\n")[0];
            var path = requestLine.Split(" ")[1];

            var response = _router.ResolveRoute(path);
            var responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}


