using Carwale.UI.Filters;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Carwale.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Carwale.Entity.CarData;
using Carwale.Notifications.Logs;
using Carwale.Entity;
using System.Text.RegularExpressions;
using Carwale.DTOs.CarData;
using System.Collections.Specialized;
using Carwale.Interfaces;
using Carwale.DTOs.NewCars;

namespace Carwale.UI.Controllers.m {
	public class MUpcomingCarListController : Controller {
		private readonly IUnityContainer _container;
		
		public MUpcomingCarListController(IUnityContainer container) {
			_container = container;
		}

		[HttpGet]
		public ActionResult UpcomingCars() {			
			try
			{
                UpcomingCarsDTO Model = null;
                UpcomingCarsInputParam inputs = new UpcomingCarsInputParam() { IsMobile = true };
				inputs.Nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString());
				inputs.RewriteUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"];

				Model = _container.Resolve<IServiceAdapterV2>("UpcomingCarsList").Get<UpcomingCarsDTO, UpcomingCarsInputParam>(inputs);

				return View("~/Views/m/New/UpcomingCarList.cshtml", Model);

			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return Redirect("/m/new/");
		}
	}
}