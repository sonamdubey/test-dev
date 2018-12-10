using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Carwale.Service
{
    public class ErrorResult : IHttpActionResult
    {
        public HttpStatusCode Status { get; }
        public ModelStateDictionary ModelState { get; }
        public string Message { get; }
        public HttpRequestMessage Request { get; }

        public ErrorResult(HttpStatusCode status, ModelStateDictionary modelState, HttpRequestMessage request)
        {
            Status = status;
            ModelState = modelState;
            Request = request;
        }

        public ErrorResult(HttpStatusCode status, string message, HttpRequestMessage request)
        {
            Status = status;
            Message = message;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = ModelState != null ? Request.CreateErrorResponse(Status, ModelState) : Request.CreateErrorResponse(Status, Message);
            return Task.FromResult(response);
        }
    }
}
