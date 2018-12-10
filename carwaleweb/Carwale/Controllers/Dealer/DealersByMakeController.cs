using Carwale.Cache.Core;
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

namespace Carwale.UI.Controllers.Dealer
{
    public class DealersByMakeController : Controller
    {
        // GET: DealersByMake
        [Route("new/{makeName}-dealers")]
        public ActionResult Index(string makeName)
        {
            int makeId = 0;
            var response = new HttpResponseMessage();

            DealerListMakeEntity Model = new DealerListMakeEntity();
            using (IUnityContainer container = new UnityContainer())
            {
                NameValueCollection Qs = Request.QueryString;

                if (Qs["makeId"] != null && CommonOpn.CheckId(Qs["makeId"]) == true)
                    Int32.TryParse(Qs["makeId"], out makeId);

                container.RegisterType<INewCarDealersCache, NewCarDealersCache>()
                    .RegisterType<ICacheProvider, MemcacheManager>()
                    .RegisterType<INewCarDealersRepository, NewCarDealersRepository>();

                INewCarDealersCache makeDealers = container.Resolve<INewCarDealersCache>();
                Model = makeDealers.GetCitiesByMake(makeId);
            }
            return View("~/Views/Dealers/ListNewCarDealersByCity.cshtml", Model);
        }
    }
}