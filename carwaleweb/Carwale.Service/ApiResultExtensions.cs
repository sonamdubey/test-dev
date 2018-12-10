using System.Net;
using System.Web.Http;

namespace Carwale.Service
{
    public static class ApiResultExtensions
    {
        public static IHttpActionResult ModelStateContent(this ApiController controller, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ErrorResult(status, controller.ModelState, controller.Request);
        }

        public static IHttpActionResult Message(this ApiController controller, string message, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ErrorResult(status, message, controller.Request);
        }
    }
}
