using Carwale.Entity.Enum;
using Carwale.Utility;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class AuthorizeSourceAttribute : AuthorizationFilterAttribute
    {
        private readonly Platform[] _allowedPlatforms;

        public AuthorizeSourceAttribute(params Platform[] allowedPlatforms)
        {
            _allowedPlatforms = allowedPlatforms;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Platform source = HttpContextUtils.GetHeader<Platform>("SourceId");
            if (source <= 0 || (_allowedPlatforms != null && _allowedPlatforms.Length > 0 && !_allowedPlatforms.Contains(source)))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid SourceId.");
            }
        }
    }
}
