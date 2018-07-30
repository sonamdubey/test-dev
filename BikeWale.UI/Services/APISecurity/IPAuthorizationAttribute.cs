using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Bikewale.Service.APISecurity
{
    /// <summary>
    /// Created by  :   Sumit Kate on 12 Sep 2017
    /// Description :   IP Authorization Attribute 
    /// This works on basic security feature. Convert ip address to base64. Compares with apikey passed in request header
    /// 
    /// IP authorization can be enabled or disabled by setting appropriate value in IsIPSecurityEnabled configuratino file
    /// </summary>
    public class IPAuthorizationAttribute : AuthorizeAttribute
    {
        private string token = string.Empty;
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains("apikey") && Utility.BWConfiguration.Instance.IsIPSecurityEnabled)
            {
                string authenticationToken = Convert.ToString(
                  actionContext.Request.Headers.GetValues("apikey").FirstOrDefault());
                if (string.IsNullOrEmpty(authenticationToken))
                {
                    base.HandleUnauthorizedRequest(actionContext);
                }
                token = authenticationToken;
                HttpContext.Current.Response.AddHeader("apikey", authenticationToken);
                base.OnAuthorization(actionContext);
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool isValid = !Utility.BWConfiguration.Instance.IsIPSecurityEnabled || Utility.EncodingDecodingHelper.EncodeTo64(Utility.CurrentUser.GetClientIP()).Equals(token);
            return isValid;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            actionContext.Response.ReasonPhrase = "Un-Authorized access";
        }
    }
}