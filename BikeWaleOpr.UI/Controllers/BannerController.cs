
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Models;
using BikewaleOpr.Models.Banner;


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

        public ActionResult Index(uint? id)
        {

            Banner objBanner = new Banner(_objBannerRespository);
            BannerVM objVM = null;
            if (objBanner != null)
            {
                uint bannerId =  id?? 0;
              
                objVM =objBanner.GetData(bannerId);
            }

            return View(objVM);
        }
     


     
    }
}