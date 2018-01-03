using BikewaleOpr.Interface.AdOperation;
using BikewaleOpr.Models;
using BikeWaleOpr.Common;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Descritpion: Controller for Ad Operations(promotion  , monetization) management
    /// </summary>
    [Authorize]
    public class AdOperationController : Controller
    {
        public readonly IAdOperation _obj = null;
        public AdOperationController(IAdOperation obj)
        {
            _obj = obj;
        }
        // GET: AdOperation
        public ActionResult Index()
        {

            AdOperation obj = new AdOperation(_obj);
            if (obj != null)
            {
                AdOperationVM objVM = new AdOperationVM();
                objVM = obj.GetData();
                return View(objVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }


        }
    }
}