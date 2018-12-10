using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCarFinder
{
    public class NewCarFinderPageController : Controller
    {
        //
        // GET: /NewCarFinder/
        public ActionResult Index()
        {
            return new FilePathResult("~/find-car/index.html", "text/html");
        }
	}
}