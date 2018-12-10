using System;
using System.Web.Mvc;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using Carwale.DTOs.NewCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Entity.CarData;
using Carwale.Notifications.Logs;

namespace Carwale.UI.Controllers.NewCars
{
    public class CarMakeController : Controller
    {

        private readonly IServiceAdapterV2 _makePageAdapter;
        private static readonly string _adDomain = System.Configuration.ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;
        public CarMakeController(Func<string, IServiceAdapterV2> adaptorFactory)
        {
            try
            {
                _makePageAdapter = adaptorFactory("MakePageDesktop");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dependency Injection Block at CarMakeController");
            }
        }
        // GET: CarMake
        [DeviceDetectionFilter]
        public ActionResult Index()
        {
            MakePageDTO_Desktop makeDTO = null;
            try
            {
                int makeId=0;
                if (Request["make"] == null || Request.QueryString["make"] == "")
                {
                    return Redirect("/new/");
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["make"]) == false)
                        return HttpNotFound();
                    makeId = Convert.ToInt32(Request.QueryString["make"]);
                }

                MakePageInputParam input = new MakePageInputParam()
                {
                    MakeId = makeId,
                    CityId = CookiesCustomers.MasterCityId,
                    CWCCookie = CurrentUser.CWC,
                };              
                makeDTO = _makePageAdapter.Get<MakePageDTO_Desktop, MakePageInputParam>(input);
                if (makeDTO.MakeDetails != null && makeDTO.MakeDetails.MakeId > 0)
                {
                    ViewBag.DomainName = _adDomain;
                    ViewBag.isExpandable = "1";
                    return View("~/Views/NewCar/MakePage.cshtml", makeDTO);
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesController.GetCarMake()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return Redirect("/new/");
        }      
    }
}