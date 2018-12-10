using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Home;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;
using Gelf4Net;

namespace Carwale.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly static string _domainName = ConfigurationManager.AppSettings["adSlotDomain"] ?? string.Empty;

        public HomeController(IUnityContainer container)
        {
            _container = container;
        }

        [CaptchaValidationFilter, DeviceDetectionFilter]
        public ActionResult Index()
        {
            HomeModel Model = new HomeModel();
            try
            {
                string cityId = CookiesCustomers.MasterCityId.ToString();
                var homePageAdaptor = _container.Resolve<IServiceAdapter>("HomePageAdaptor");
                Model = homePageAdaptor.Get<HomeModel>(cityId);

                ViewBag.City = Model.IPToLocation;
                ViewBag.CityId = cityId;
                if (Model.NewCarPlaceHolder != null)
                {
                    ViewData["HomePageBanner"] = Model.NewCarPlaceHolder.Ad_Html;
                }

                if (Model.PQPlaceHolder != null)
                {
                    ViewData["PQWidget"] = Model.PQPlaceHolder.Ad_Html;
                }
                ViewBag.UsedCity = Model.UsedCity;
                ViewBag.PageId = 1;
                ViewBag.DomainName = _domainName;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomeController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return View("~/Views/HomePage.cshtml", Model);
        }


    }
}