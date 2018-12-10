using Carwale.Notifications.Logs;
using System;
using System.Web.Http;
using Newtonsoft.Json;

namespace Carwale.Service.Controllers.Classified
{
   public class ErrorController : ApiController
    {
       [HttpPost]
       [Route("api/exceptions/")]
       public IHttpActionResult ClientException([FromBody] object exception)
       {
           var ex = new Exception(JsonConvert.SerializeObject(exception));
           Logger.LogException(ex, "ClientException");
           return Ok(); 
       }
    }
}
