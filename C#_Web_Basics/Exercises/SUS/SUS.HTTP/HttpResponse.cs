using System;
using System.Collections.Generic;
using System.Text;
using SUS.HTTP.Enums;

namespace SUS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(string contentType, byte[] body,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this.StatusCode = statusCode;
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
            this.Headers = new List<Header>
            {
                { new Header("Content-Type", contentType) },
                { new Header("Content-Length", body.Length.ToString()) } //TODO: Check if utf8 encoding length should be used here
            };
            this.Cookies = new List<Cookie>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public byte[] Body { get; set; }

        public override string ToString()
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NewLine);

            foreach (var header in this.Headers)
            {
                responseBuilder.Append(header + HttpConstants.NewLine);
            }

            foreach (var cookie in this.Cookies)
            {
                responseBuilder.Append("Set-Cookie: " + cookie + HttpConstants.NewLine);
            }

            responseBuilder.Append(HttpConstants.NewLine);

            return responseBuilder.ToString();
        }
    }
}