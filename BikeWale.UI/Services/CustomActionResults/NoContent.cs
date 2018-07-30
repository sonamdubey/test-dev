using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bikewale.Service.CustomActionResults
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 7 Sept 2015
    /// Summary : Function to return the no content found status with the message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NoContent<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly T _message;
        
        public NoContent(HttpRequestMessage request, T message)
        {
            _request = request;
            _message = message;
        }

        public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.NoContent, _message);
            return Task.FromResult(response);
        }
    }
}