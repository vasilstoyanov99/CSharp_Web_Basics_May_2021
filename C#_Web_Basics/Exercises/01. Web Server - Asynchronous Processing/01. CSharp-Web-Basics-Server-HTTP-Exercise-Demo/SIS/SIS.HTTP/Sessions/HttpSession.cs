using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        public HttpSession(string id)
        {
            this.Id = id;
        }

        public string Id { get; }

        public object GetParameter(string name)
        {
            throw new NotImplementedException();
        }

        public bool ContainsParameter(string name)
        {
            throw new NotImplementedException();
        }

        public void AddParameter(string name, object parameter)
        {
            throw new NotImplementedException();
        }

        public void ClearParameters()
        {
            throw new NotImplementedException();
        }
    }
}
