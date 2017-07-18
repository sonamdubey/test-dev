using BikewaleOpr.Entity;
using BikewaleOpr.Models;
using BikewaleOpr.Models.Banner;
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

            Banner objBanner = new Banner();
            BannerVM objVM = null;
            if (objBanner != null)
            {
                objVM=objBanner.GetData();
            }

            return View(objVM);
        }
        //public ActionResult SaveBanner([System.Web.Http.FromBody] BannerDetails objBanner)
        //{



        //}
    }
}