using Carwale.Notifications.Logs;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly string[] _requiredParams;

        public ValidateModelAttribute(params string[] requiredParams)
        {
            _requiredParams = requiredParams;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            var arguments = actionContext.ActionArguments;
            foreach (string param in _requiredParams)
            {
                object value;
                if (!arguments.TryGetValue(param, out value) || value == null)
                {
                    modelState.AddModelError(param, $"'{param}' cannot be null.");
                }
            }

            if (!modelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
        }
    }
}
