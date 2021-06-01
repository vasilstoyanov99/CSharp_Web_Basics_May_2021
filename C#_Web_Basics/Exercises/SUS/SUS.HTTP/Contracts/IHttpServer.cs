using System;
using System.Threading.Tasks;

namespace SUS.HTTP.Contracts
{
    public interface IHttpServer
    {
        void AddRoute(string path, Func<HttpRequest, HttpResponse> action);

        Task StartAsync(int port);
    }
}
