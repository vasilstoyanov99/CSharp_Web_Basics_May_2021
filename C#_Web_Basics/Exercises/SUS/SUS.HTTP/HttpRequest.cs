using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUS.HTTP.Enums;

namespace SUS.HTTP
{
    public class HttpRequest 
    {
        public HttpRequest(string requestAsString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();

            var split = requestAsString.Split(HttpConstants.NewLine);
            var header = split[0];
            var headerParts = header.Split(' ');
            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerParts[0]);
            this.Path = headerParts[1];

            var lineIndex = 1;
            bool isHeader = true;
            var bodyBuilder = new StringBuilder();

            while (lineIndex < split.Length)
            {
                var currLine = split[lineIndex];
                lineIndex++;

                if (String.IsNullOrWhiteSpace(currLine))
                {
                    isHeader = false;
                    continue;
                }

                if (isHeader)
                {
                    this.Headers.Add(new Header(currLine));
                }
                else
                {
                    bodyBuilder.AppendLine(currLine);
                }
            }

            if (this.Headers.Any(x => x.Name == HttpConstants.RequestCookieHeader))
            {
                var cookiesAsString = this.Headers
                    .FirstOrDefault(x => x.Name == HttpConstants.RequestCookieHeader).Value;
                var cookies = cookiesAsString.Split("; ",
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var data in cookies)
                {
                    this.Cookies.Add(new Cookie(data));
                }
            }

            this.Body = bodyBuilder.ToString();
        }

        public string Path { get; set; }

        public string Body { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }
    }
}