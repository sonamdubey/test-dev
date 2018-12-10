using Carwale.Entity.Classified.SellCarUsed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters.ExceptionFilters.Classified
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SellCarApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings = { NullValueHandling = NullValueHandling.Ignore,
                                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                   DefaultValueHandling = DefaultValueHandling.Ignore
                                  }
        };
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new ModalPopUp
                {
                    Heading = "Error!",
                    Description = "Something went wrong. Please try again."
                }, _jsonFormatter);
            }
        }
    }
}
