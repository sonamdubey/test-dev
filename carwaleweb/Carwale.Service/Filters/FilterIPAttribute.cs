using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FilterIPAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the configuration key for allowed single IPs
        /// </summary>
        /// <value>The configuration key.</value>
        public string AllowedIPs { get; set; }
        private bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            string userIpAddress = UserTracker.GetUserIp();
                
            return IsIpAllowed(userIpAddress); // Check that the IP is allowed to access
        }

        /// <summary>
        /// Checks the allowed IPs.
        /// </summary>
        /// <param name="userIpAddress">The user ip address.</param>
        /// <returns></returns>
        private bool IsIpAllowed(string userIpAddress)
        {
            if (!string.IsNullOrEmpty(AllowedIPs))
            {
                string allowedIPs = ConfigurationManager.AppSettings[AllowedIPs];
                if (!string.IsNullOrEmpty(allowedIPs))
                {
                    return allowedIPs.Split(',').ToList<string>().Exists(ip => ip == userIpAddress);
                }
            }
            return false;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext == null)
                {
                    throw new ArgumentNullException("actionContext");
                }

                if (AuthorizeCore((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]))
                {
                    return;
                }
                base.HandleUnauthorizedRequest(actionContext);
            }
            catch (Exception ex)
            {
                 Logger.LogException(ex);
            }            
        }
    }
}
