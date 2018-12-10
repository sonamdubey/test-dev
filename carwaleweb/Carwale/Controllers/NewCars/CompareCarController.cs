using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCars
{
    public class CompareCarController : Controller
    {
        private readonly IUnityContainer _container;
        public CompareCarController(IUnityContainer container)
        {
            _container = container;
        }
        [DeviceDetectionFilter, Route("comparecars/")]
        public ActionResult CompareCars()
        {
            var compareCarModel = new CompareCarsModel();
            try
            {
                _container.RegisterInstance<int>(CookiesCustomers.MasterCityId);
                _container.RegisterInstance<System.Collections.Specialized.NameValueCollection>(Request.QueryString);

                var comparePageAdaptor = _container.Resolve<IServiceAdapter>("CompareCarAdapter");
                compareCarModel = comparePageAdaptor.Get<CompareCarsModel>();
                int widgetSource = (int)WidgetSource.CompareCarLandingCompareCarWidgetDesktop;
                if (compareCarModel.HotComparisons != null && compareCarModel.HotComparisons.Count > 0)
                    compareCarModel.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);
                compareCarModel.BreadcrumbEntitylist = BindBreadCrumb(compareCarModel);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CompareCarController.CompareCars()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return View("~/Views/NewCar/CompareCar.cshtml", compareCarModel);
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(CompareCarsModel compareCarModel)
        {
            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "New Cars", Link = "/new/", Text = "New Cars" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = "Compare Cars in India" });
            return _BreadcrumbEntitylist;
        }
    }
}