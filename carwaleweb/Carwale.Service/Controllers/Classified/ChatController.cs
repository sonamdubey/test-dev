using Carwale.DTOs.Classified;
using Carwale.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
    public class ChatController : ApiController
    {
        [HandleException, LogApi, ValidateModel("chatError"),Route("api/chat/errors/"), HttpPost]
        public IHttpActionResult Post(ChatError chatError)
        {
            return Ok();
        }
    }
}
