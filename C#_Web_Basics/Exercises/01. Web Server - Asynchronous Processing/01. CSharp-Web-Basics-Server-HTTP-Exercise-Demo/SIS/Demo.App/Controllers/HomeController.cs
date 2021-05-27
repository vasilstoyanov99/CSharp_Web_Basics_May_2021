using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;

namespace Demo.App.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Home(IHttpRequest httpRequest)
        {
            return this.View();
        }

        
    }
}
