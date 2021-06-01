using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SUS.HTTP.Contracts;
using HttpStatusCode = SUS.HTTP.Enums.HttpStatusCode;

namespace SUS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private IDictionary<string, Func<HttpRequest, HttpResponse>> _routeTable =
            new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        {
            if (!_routeTable.ContainsKey(path))
            {
                _routeTable[path] = action;
            }
            else
            {
                _routeTable.Add(path, action);
            }
        }

        public async Task StartAsync(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                ProcessClientAsync(client);
            }
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            { 
                using var networkStream = client.GetStream();

                var data = new List<byte>();
                var buffer = new byte[HttpConstants.BufferSize];
                var position = 1;

                while (true)
                {
                    var readCount = await networkStream
                        .ReadAsync(buffer, position, buffer.Length);
                    position += readCount;

                    if (readCount < buffer.Length)
                    {
                        var partialBuffer = new byte[readCount];
                        Array.Copy(buffer, partialBuffer, partialBuffer.Length);
                        data.AddRange(partialBuffer);
                        break;
                    }
                    else
                    {
                        data.AddRange(buffer);
                    }

                    var requestAsString = Encoding.UTF8.GetString(data.ToArray());
                    var request = new HttpRequest(requestAsString);
                    Console.WriteLine($"{request.Method} {request.Path} => {request.Headers.Count} headers");

                    HttpResponse response;

                    if (this._routeTable.ContainsKey(request.Path))
                    {
                        var action = _routeTable[request.Path];
                        response = action(request);
                    }
                    else
                    {
                        response = new HttpResponse("text/html", new byte[0],
                            HttpStatusCode.NotFound);
                    }

                    response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                    { HttpOnly = true, MaxAge = 60 * 24 * 60 * 60 });
                    response.Headers.Add(new Header("Server", "SUS Server 1.0"));
                    var responseHeadersInBytes = Encoding.UTF8.GetBytes(response.ToString());
                    await networkStream.WriteAsync
                        (responseHeadersInBytes, 0, responseHeadersInBytes.Length);
                    await networkStream.WriteAsync(response.Body, 0, response.Body.Length);

                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
        }
    }
}
