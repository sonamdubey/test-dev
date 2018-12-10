using System;
using System.Web.Mvc;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using Carwale.DTOs.NewCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Service;
using Carwale.Interfaces.CarData;
using Carwale.Entity;
using System.Text.RegularExpressions;
using System.Web;
using Carwale.Interfaces.CMS.Articles;
using System.Collections.Generic;
using System.Collections.Specialized;
using Carwale.Notifications.Logs;

namespace Carwale.UI.Controllers.NewCars {
	public class UpcomingCarsController : Controller {

		private readonly IUnityContainer _container;
		
		public UpcomingCarsController(IUnityContainer container) {
			_container = container;
		}
		
		[DeviceDetectionFilter]
		public ActionResult Index() {
			UpcomingCarsDTO Model = new UpcomingCarsDTO();
			try
			{
				UpcomingCarsInputParam inputs = new UpcomingCarsInputParam() { IsMobile = false };
				inputs.Nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString());
				inputs.RewriteUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"];

				Model = _container.Resolve<IServiceAdapterV2>("UpcomingCarsList").Get<UpcomingCarsDTO, UpcomingCarsInputParam>(inputs);

				return View("~/Views/NewCar/UpcomingCars.cshtml", Model);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return Redirect("/new/");
		}
	}
}