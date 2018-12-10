using Carwale.Entity.ViewModels;
using Carwale.Notifications;
using Carwale.UI.Filters;
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
using Carwale.Interfaces.CompareCars;
using Carwale.Entity;
using Carwale.Entity.CompareCars;
using Carwale.DTOs.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Common;
using Carwale.BL.Experiments;

namespace Carwale.UI.Controllers.m {
	public class MCompareCarController : Controller {
		private readonly IUnityContainer _container;
		private System.Collections.Specialized.NameValueCollection versionIds;
        private const ushort _pageSize = 10;
		public MCompareCarController(IUnityContainer container) {
			_container = container;
		}
		// GET: MCompareCar
		[Route("m/comparecars/")]
		public ActionResult CompareCars() {
			var compareCarsModel = new CompareCarsModel();
			List<string> models = new List<string>();
			versionIds = new System.Collections.Specialized.NameValueCollection();
			int versionId;
			try
			{
				if (!string.IsNullOrWhiteSpace(Request.QueryString["car1"]) && int.TryParse(Request.QueryString["car1"], out versionId)) versionIds.Add("car1", Request.QueryString["car1"]);
				else if (!string.IsNullOrWhiteSpace(Request.QueryString["model1"])) models.Add(Request.QueryString["model1"]);
				if (!string.IsNullOrWhiteSpace(Request.QueryString["car2"]) && int.TryParse(Request.QueryString["car2"], out versionId)) versionIds.Add("car2", Request.QueryString["car2"]);
				else if (!string.IsNullOrWhiteSpace(Request.QueryString["model2"])) models.Add(Request.QueryString["model2"]);

				if (versionIds.Count == 0 && models.Any())
				{
					ICarModelCacheRepository _carmodelCache = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
                    ICarModels _carModelBl = UnityBootstrapper.Resolve<ICarModels>();

                    int c = 1; 
					foreach (var model in models) { 
					var maskingResponse = _carModelBl.FetchModelIdFromMaskingName(model,string.Empty);
						if (maskingResponse.ModelId > 0) { 
							versionIds.Add("car" + c.ToString(), _carmodelCache.GetModelDetailsById(maskingResponse.ModelId).PopularVersion.ToString());
							c++;
						}
					}
				}
				
				_container.RegisterInstance<System.Collections.Specialized.NameValueCollection>(versionIds);
				_container.RegisterInstance<int>(CookiesCustomers.MasterCityId);
				var comparePageAdaptor = _container.Resolve<IServiceAdapter>("CompareCarAdapter");
				compareCarsModel = comparePageAdaptor.Get<CompareCarsModel>();
                int widgetSource = (int)WidgetSource.CompareCarLandingCompareCarWidgetMobile;
                if (compareCarsModel != null && compareCarsModel.HotComparisons != null && compareCarsModel.HotComparisons.Count > 0)
                    compareCarsModel.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);
                if (compareCarsModel.VersionsDetails == null) compareCarsModel.VersionsDetails = new List<CarVersionDetails>();
                compareCarsModel.ExperimentAdSlot = ProductExperiments.GetCompareCarsExperimentAdSlot(CookiesCustomers.AbTest);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return View("~/Views/m/New/Comparecar.cshtml", compareCarsModel);
		}

        [Route("m/comparecars/all/"),Route("m/comparecars/all/page/{pageNumber?}")]
        public ActionResult AllCompareCars(ushort pageNumber = 1)
        {
            ICompareCarsCacheRepository compareBL = _container.Resolve<ICompareCarsCacheRepository>();
            List<HotCarComparison> compareList = compareBL.GetHotComaprisons(50);
            int count = compareList.Count;

            CompareAllDTO dto = new CompareAllDTO();
            dto.CompareList = compareList.Skip((pageNumber - 1) * _pageSize).Take(_pageSize).ToList();
            
            dto.TotalPages = count/_pageSize;
            if (count % _pageSize != 0)
                dto.TotalPages++;

            if(pageNumber > dto.TotalPages || pageNumber.Equals(0))
            {
                return HttpNotFound();
            }

            dto.CurrentPage = pageNumber;
            int widgetSource = (int)WidgetSource.AllCompareCarPageMobile;
            if (dto.CompareList != null && dto.CompareList.Count > 0)
                dto.CompareList.ForEach(x => x.WidgetPage = widgetSource);
            dto.MetaData = new PageMetaTags
            {
                Title = "Compare Cars | New Car Comparisons in India - CarWale",
                Description = "Comparing Indian cars was never this easy. CarWale presents you the easiest way of comparing cars. Choose two or more cars to compare them head-to-head.",
                Canonical = "https://www.carwale.com/comparecars/"
            };

            return View("~/Views/m/New/ComparecarAll.cshtml", dto);
        }

	}
}