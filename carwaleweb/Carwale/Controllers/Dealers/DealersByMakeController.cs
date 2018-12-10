using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Entity.Dealers;
using System.Net.Http;
using System.Collections.Specialized;
using Carwale.Utility;
using System.Web.Http.Routing;
using Carwale.UI.Common;
using System.Globalization;
using Carwale.Cache.CarData;
using Carwale.DAL.CarData;
using System.Web.SessionState;
using Carwale.UI.Filters;

namespace Carwale.UI.Controllers.Dealer
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class DealersByMakeController : Controller
    {
        private readonly IUnityContainer _container;

        public DealersByMakeController(IUnityContainer container)
        {
            _container = container;
        }

        // GET: DealersByMake
        [DeviceDetectionFilter("/research/locatedealerpopup.aspx")]
        //[Route("new/{makeName}-dealers")]
        public ActionResult Index(string makeName)
        {
            int makeId = 0;
            var response = new HttpResponseMessage();

            DealerListMakeEntity Model = new DealerListMakeEntity();
            try
            {
                {
                    NameValueCollection Qs = Request.QueryString;

                    if (Qs["makeId"] != null && CommonOpn.CheckId(Qs["makeId"]) == true)
                        Int32.TryParse(Qs["makeId"], out makeId);

                    INewCarDealers makeDealers = _container.Resolve<INewCarDealers>();
                    ICarMakesCacheRepository makeDetails = _container.Resolve<ICarMakesCacheRepository>();

                    Model.DealersByMake = makeDealers.GetStatesAndCitiesByMake(makeId);
                    Model.CarDetails = makeDetails.GetCarMakeDetails(makeId);
                    Model.MakeId = makeId;
                    Model.BreadcrumbEntitylist = BindBreadCrumb(Model.CarDetails.MakeName);
                }
            }
            catch (Exception ex)
            {
                Carwale.Notifications.ExceptionHandler objErr = new Carwale.Notifications.ExceptionHandler(ex, "DealerListingController.ListOfStateCityByMake()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            if (Model != null && Model.DealersByMake != null && Model.DealersByMake.Count > 0)
            {
                return View("~/Views/Dealers/ListNewCarDealersByCity.cshtml", Model);
            }
            else
            {
                return Redirect("/new/locatenewcardealers.aspx");
            }
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(string makeName)
        {
            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "New Cars", Link = "/new/", Text = "New Cars" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title ="New Car Dealers", Link = "/new/locatenewcardealers.aspx", Text = "New Car Dealers" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = string.Format("{0} Dealers/Showrooms",makeName)});
            return _BreadcrumbEntitylist;
        }
    }
}