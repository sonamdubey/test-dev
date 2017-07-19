using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Banner;
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
        private readonly IBannerRepository _objBannerRespository = null;
        public BannerController(IBannerRepository objBannerRespository)
        {
            _objBannerRespository = objBannerRespository;
        }

        public ActionResult Index(uint? bannerId)
        {

            Banner objBanner = new Banner(_objBannerRespository);
            BannerVM objVM = null;
            if (objBanner != null)
            {
                uint id = bannerId ?? 0;
                objVM=objBanner.GetData(id);
            }

            return View(objVM);
        }
        [HttpPost, Route("submit/")]
        public ActionResult SaveBanner([System.Web.Http.FromBody] BannerVM objBanner)
        {


            return Redirect("/Banner/Index");
        }
    }
}