namespace WebFramework.Routing
{
    public class Router
    {
        private readonly Dictionary<string, Func<string>> _routes = new Dictionary<string, Func<string>>();

        public void RegisterRoute(string path, Func<string> action)
        {
            _routes[path] = action;
        }

        public string ResolveRoute(string path)
        {
            if (_routes.ContainsKey(path))
            {
                return _routes[path]();
            }

            return "HTTP/1.1 404 Not Found\r\n\r\nPage not found";
        }
    }
}
