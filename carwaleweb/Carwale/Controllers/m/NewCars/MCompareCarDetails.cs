using Microsoft.Practices.Unity;
using Carwale.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carwale.Interfaces.NewCars;
using Carwale.Entity.CompareCars;
using Carwale.Service;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using Carwale.DTOs.CarData;
using Carwale.Utility;
using Carwale.Notifications.Logs;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.Prices;
using Carwale.Entity.Common;
using Carwale.BL.Experiments;

namespace Carwale.UI.Controllers.m
{
    public class MCompareCarDetailController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ITyresBL _tyres;
        protected List<int> versionIds = new List<int>();
        string redirectUrl = null;
        string defaultLandingRedirect = "/m/comparecars/";
        private readonly IEmiCalculatorAdapter _emiCalculatorAdapter;

        public MCompareCarDetailController(IUnityContainer container, ITyresBL tyres, IEmiCalculatorAdapter emiCalculatorAdapter)
        {
            _container = container;
            _tyres = tyres;
            _emiCalculatorAdapter = emiCalculatorAdapter;
        }

        public ActionResult Compare()
        {

            CompareDetailsModel carData = new CompareDetailsModel();
            try
            {
                if (!ProcessQueryString(Request)) return Redirect(defaultLandingRedirect);
                if (redirectUrl != null) return RedirectPermanent(redirectUrl);
                Location custLocation = new Location
                {
                    CityId = CookiesCustomers.MasterCityId,
                    ZoneId = CookiesCustomers.MasterZoneId,
                    AreaId = CookiesCustomers.MasterAreaId

                };
                CompareCarInputParam input = new CompareCarInputParam { VersionIds = versionIds, CustLocation = custLocation };
                IServiceAdapterV2 comparePageAdapter = _container.Resolve<IServiceAdapterV2>("CompareCarDetailsAdapterMobile");
                carData = comparePageAdapter.Get<CompareDetailsModel, CompareCarInputParam>(input);
                carData.VersionsWithTyres = _tyres.CheckForTyres(versionIds);
                carData.ExperimentAdSlot = ProductExperiments.GetCompareCarsExperimentAdSlot(CookiesCustomers.AbTest);
                if (carData != null)
                    return View("~/Views/m/New/ComparecarDetails.cshtml", carData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Redirect("/m/comparecars/");
        }

        public bool ProcessQueryString(HttpRequestBase Request)
        {

            var QS = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            QS.Remove("mo");
            if (QS.ToString().Contains("car"))
            {
                QS.Remove("car1"); QS.Remove("car2");
                redirectUrl = HttpUtility.UrlDecode(Request.RawUrl.Split('?')[0] + (QS.Count > 0 ? "?" + QS.ToString() : string.Empty));
            }
            if (new System.Text.RegularExpressions.Regex(@"c[1-2]=").IsMatch(Request.QueryString.ToString()))
            {
                int versionId;
                for (int i = 1; i <= 2; i++)
                {
                    if (int.TryParse(QS["c" + i], out versionId))
                    {
                        if (versionIds.Contains(versionId)) { defaultLandingRedirect += "?car1=" + versionId; return false; }
                        else versionIds.Add(versionId);
                    }
                    else return false;
                }
            }
            bool getVersionIdsFromMaskingname = !versionIds.Any();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("mo")))
            {
                List<Tuple<int, string>> list = new List<Tuple<int, string>>();
                ICarModelCacheRepository _carmodelCache = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
                string[] maskingNames = Request.QueryString.Get("mo").Split(',');

                foreach (string maskingName in maskingNames)
                {
                    Entity.CarModelMaskingResponse resp = _carmodelCache.GetModelByMaskingName(maskingName);
                    if (getVersionIdsFromMaskingname)
                    {
                        CarModelDetails car = _carmodelCache.GetModelDetailsById(resp.ModelId);
                        if (car.PopularVersion > 0)
                        {
                            if (versionIds.Contains(car.PopularVersion)) { redirectUrl = ManageCarUrl.CreateModelUrl(car.MakeName, car.MaskingName); return true; }
                            else versionIds.Add(car.PopularVersion);
                        }
                    }
                    list.Add(new Tuple<int, string>(resp.ModelId, Format.RemoveSpecialCharacters(resp.MakeName) + "-" + resp.MaskingName));
                }
                if (versionIds.Count < 2) return false;
                if (!list.Select(t => t.Item1).IsInDescOrder()) redirectUrl = HttpUtility.UrlDecode(string.Format("/m/comparecars/{0}/{1}", Format.GetCompareUrl(list), (QS.Count > 0 ? "?" + QS.ToString() : string.Empty)));
            }
            else return false;

            return true;
        }
    }

}