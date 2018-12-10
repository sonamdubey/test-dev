using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters.AuthorizationFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private readonly string _tokenToValidate;
        private readonly string _authenticationHeader;

        /// <summary>
        /// Validate secretToken in the authentication header 
        /// </summary>
        /// <param name="tokenToValidate"></param>
        /// <param name="authenticationHeader"></param>
        public BasicAuthenticationAttribute(string tokenToValidate, string authenticationHeader)
        {
            byte[] byteArr = Encoding.UTF8.GetBytes(tokenToValidate);
            _tokenToValidate = Convert.ToBase64String(byteArr);
            _authenticationHeader = authenticationHeader;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string[] authenticationHeader = HttpContextUtils.GetHeader<string>(_authenticationHeader).Split(' ');

            if (authenticationHeader != null && authenticationHeader.Length > 1 && authenticationHeader[0].Equals("Basic", StringComparison.InvariantCultureIgnoreCase))
            {
                string acessToken = authenticationHeader[1];
                if (acessToken != _tokenToValidate)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
