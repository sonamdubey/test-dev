using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;

namespace BikeWaleOpr.MVC.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomePage _IHomePage;

        public HomeController(IHomePage homePage)
        {
            _IHomePage = homePage;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 09-03-2017
        /// Description : Added functionality for user notification panel.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HomePageData objHomePageData = null;
            try
            {
                objHomePageData = _IHomePage.GetHomePageData(CurrentUser.Id, CurrentUser.UserName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "HomeController/Index");
            }
            return View(objHomePageData);
        }

    }
}