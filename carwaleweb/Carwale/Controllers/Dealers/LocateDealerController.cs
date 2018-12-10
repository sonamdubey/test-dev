using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Entity.Dealers;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.Dealers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class LocateDealerController : Controller
    {
        private readonly IUnityContainer _container;

        public LocateDealerController(IUnityContainer container)
        {
            _container = container;
        }

        // GET: Default
        [DeviceDetectionFilter("/research/locatedealerpopup.aspx")]
        [Route("new/locatenewcardealers.aspx")]
        public ActionResult Index()
        {            
            LocateDealerModel Model = new LocateDealerModel();

            try
            {
                INewCarDealersCache makeDealers = _container.Resolve<INewCarDealersCache>();
                Model.MakeDealers = makeDealers.NewCarDealerCountMake("");
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LocateDealerController Index()\n Exception :" + ex.Message);
            }
            return View("~/Views/Dealers/LocateDealer.cshtml", Model);
        }
    }
}