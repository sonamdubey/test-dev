using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class CompareBikesController : Controller
    {
        // GET: CompareBikes
        [Route("compare/")]
        public ActionResult Index()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}