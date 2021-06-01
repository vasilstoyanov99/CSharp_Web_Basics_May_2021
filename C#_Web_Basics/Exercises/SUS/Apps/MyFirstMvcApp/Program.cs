using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUS.HTTP;
using SUS.HTTP.Contracts;
using SUS.HTTP.Enums;

namespace MyFirstMvcApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();
            server.AddRoute("/", HomePage);
            server.AddRoute("/favicon.ico", Favicon);
            server.AddRoute("about", About);

            await server.StartAsync(90);
        }

        static HttpResponse HomePage(HttpRequest request)
        {
            var responseHtml = "<h1>Welcome!</h1>" +
                               request.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value;
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);
            return response;
        }

        static HttpResponse Favicon(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        static HttpResponse About(HttpRequest request)
        {
            var responseHtml = "<h1>About...</h1>";
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);
            return response;
        }
    }
}
