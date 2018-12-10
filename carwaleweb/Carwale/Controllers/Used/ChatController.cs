using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Used
{
    public class ChatController : Controller
    {
        [HttpGet, Route("used/chat/")]
        [OutputCache(Duration = 1800)]
        public ActionResult Chat()
        {
            return PartialView("~/Views/m/Used/_chat.cshtml");
        }
    }
}