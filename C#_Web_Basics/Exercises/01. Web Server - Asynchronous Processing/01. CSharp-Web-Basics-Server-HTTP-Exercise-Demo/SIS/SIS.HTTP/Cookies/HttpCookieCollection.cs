using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Cookies.Contracts;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private Dictionary<string, HttpCookie> _cookiesCollection;

        public HttpCookieCollection()
        {
            _cookiesCollection = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie cookie)
        {
            _cookiesCollection.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            return _cookiesCollection.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            return _cookiesCollection[key];
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return _cookiesCollection.Values.GetEnumerator();
        }

        public bool HasCookies()
        {
            return _cookiesCollection.Any();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this._cookiesCollection.Values);
        }
    }
}
