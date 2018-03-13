using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Models;

namespace Bikewale.Controllers
{
    public class AdTestController : Controller
    {
        // GET: AdTest
        public ActionResult Index()
        {
			ModelBase objBase = new ModelBase();			

			return View(objBase);
        }
    }
}