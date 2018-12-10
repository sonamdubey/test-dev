using AEPLCore.Security;
using AEPLCore.Utils.ClientTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Filters.AuthorizationFilters
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _securityToken = "token";
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(ControllerContext actionContext)
        {
            try
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;
                string token = request.Params[_securityToken];
                return TokenManager.ValidateToken(token, IpTracker.CurrentUserIp, request.UserAgent);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}