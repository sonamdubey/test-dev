using Carwale.Utility;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Carwale.Service.Filters
{
    public class PlatformRequired : AuthenticateBasicAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.GetValueFromHttpHeader<int>("sourceid") == 0)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Platform source was missing");
                actionContext.Response = response;
            }
        }
    }
}
