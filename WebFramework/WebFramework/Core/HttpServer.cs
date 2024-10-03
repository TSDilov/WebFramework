namespace WebFramework.Core
{
    using System.Net.Sockets;
    using System.Net;
    using System.Text;

    public class HttpServer
    {
        private readonly TcpListener _listener;

        public HttpServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
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

            Console.WriteLine(request);

            var response = "HTTP/1.1 200 OK\r\n" +
                           "Content-Type: text/plain\r\n" +
                           "Content-Length: 11\r\n" +
                           "\r\n" +
                           "Hello World";

            var responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}


