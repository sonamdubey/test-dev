using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
    }
}