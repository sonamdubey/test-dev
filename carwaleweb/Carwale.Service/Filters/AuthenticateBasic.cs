using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http.Headers;
using System.Net;
using Carwale.DAL.Security;
using Carwale.Interfaces;
using Microsoft.Practices.Unity;
using Carwale.Cache.Security;
using Carwale.Cache.Core;
using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Service.Filters
{
    /// <summary>
    /// Created By : Supriya K on 12/6/2014
    /// Class to validate SourceId against CWK key for any request coming to api
    /// </summary>
    public class AuthenticateBasicAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool isAuthenticated = IsAuthenticated(actionContext);

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Content = new StringContent("UnAuthorized Request.");
                actionContext.Response = response;
            }
        }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            var segments = actionContext.Request.RequestUri.Segments;
            var platformId = GetHttpRequestHeader(headers, "platformid");
            var CWKString = GetHttpRequestHeader(headers, "CWK");
            var sourceIdString = GetHttpRequestHeader(headers, "SourceId");
            if (platformId == "2" && segments.Count() > 2 && segments[1] == "api/" && segments[2] == "insurance/")
                return true;
            if (string.IsNullOrEmpty(CWKString))
                return false;
            else if (string.IsNullOrEmpty(sourceIdString))
                return false;

            using(IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ISecurityRepository<bool>, SecurityRepositoryCache<bool>>()
                    .RegisterType <ICacheManager, CacheManager>();

                ISecurityRepository<bool> _securityRepo = container.Resolve<ISecurityRepository<bool>>();

                return _securityRepo.IsValidSource(sourceIdString, CWKString);
            
            }
        }

        private static string GetHttpRequestHeader(HttpHeaders headers, string headerName)
        {
            if (!headers.Contains(headerName))
                return string.Empty;
            return ((string[])(headers.GetValues(headerName)))[0];
        }
    }
}
