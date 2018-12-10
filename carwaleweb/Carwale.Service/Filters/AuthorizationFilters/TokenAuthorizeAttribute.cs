using AEPLCore.Security;
using AEPLCore.Utils.ClientTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Carwale.Service.Filters.AuthorizationFilters
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _securityToken = "token";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        private static bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                HttpRequestMessage request = actionContext.Request;
                string token = request.GetQueryNameValuePairs().Where(x => x.Key == _securityToken).Select(x => x.Value).FirstOrDefault();
                return TokenManager.ValidateToken(token, IpTracker.CurrentUserIp, request.Headers.UserAgent.ToString());
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
