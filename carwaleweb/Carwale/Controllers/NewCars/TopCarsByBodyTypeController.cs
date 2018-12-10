using Carwale.BL.Experiments;
using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.NewCars
{
    public class TopCarsByBodyTypeController : Controller
    {

        private readonly IServiceAdapterV2 _topCarByBodyTypeAdapter;
        private readonly INewCarSearchAppAdapter _adapter;
        private readonly INewCarElasticSearch _elasticsearch;
        private readonly ITopCarsBl _topCarBl;
        public TopCarsByBodyTypeController(Func<string, IServiceAdapterV2> factory, INewCarSearchAppAdapter adapter, INewCarElasticSearch elasticsearch, ITopCarsBl topCarBl)
        {
            _topCarByBodyTypeAdapter = factory("TopCarsByBodyTypeAdaptor");
            _adapter = adapter;
            _elasticsearch = elasticsearch;
            _topCarBl = topCarBl;
        }

        [Route("best-{bodyType:regex([A-Za-z])}s-in-india/")]
        public ActionResult Index(string bodyType)
        {
            try
            {
                TopCarsByBodyTypeDto topCarsByBodyType = null;
                NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.Url.Query);

                CarBodyStyle bodyStyleId;
                Enum.TryParse(bodyType, true, out bodyStyleId);
                short carBodyType = (short)bodyStyleId;
                ViewBag.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                if (nvc != null && nvc.HasKeys() && ViewBag.IsMobile)
                {
                    nvc["pageNo"] = "1";
                    nvc["pageSize"] = "5";
                    NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(nvc);
                    inputs.IsMobile = ViewBag.IsMobile;
                    topCarsByBodyType = _topCarByBodyTypeAdapter.Get<TopCarsByBodyTypeDto, NewCarSearchInputs>(inputs);
                    topCarsByBodyType.IsFilterApplied = true;
                    string queryString = string.Empty;
                    topCarsByBodyType.FilterAppliedCount = getFilterAppliedCount(nvc);
                    if (topCarsByBodyType.TotalModels > Convert.ToInt32(nvc["pageSize"]) * Convert.ToInt32(nvc["pageNo"]))
                    {
                        topCarsByBodyType.NextPageUrl = string.Format("/topcars/filtered/?{0}", getQueryString(nvc));
                    }
                }
                else if (Enum.TryParse(bodyType, true, out bodyStyleId))
                {
                    TopCarsByBodyTypeParams input = new TopCarsByBodyTypeParams()
                    {
                        Count = CWConfiguration.TopCarByBodyTypeCount,
                        BodyType = carBodyType > 0 ? carBodyType : (short)CarBodyStyle.Hatchback,
                        CityId = CookiesCustomers.MasterCityId,
                        ZoneId = CookiesCustomers.MasterZoneId,
                        PageNo = 1,
                        IsMobile = ViewBag.IsMobile
                    };
                    topCarsByBodyType = _topCarByBodyTypeAdapter.Get<TopCarsByBodyTypeDto, TopCarsByBodyTypeParams>(input);
                    topCarsByBodyType.FilterAppliedCount = 1;
                }
                else
                {
                    return new HttpNotFoundResult("Invalid Body Type");
                }
                SetPageProperties(topCarsByBodyType, bodyStyleId, carBodyType);
                return View("~/Views/NewCar/TopCarsByBodyType.cshtml", topCarsByBodyType);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "TopCarsByBodyTypeController.Index()\n Exception : " + ex.Message);
            }
            return new EmptyResult();
        }

        [Route("topcars/filtered/")]
        public ActionResult TopCars()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc = HttpUtility.ParseQueryString(this.Request.Url.Query);
            NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(nvc);
            try
            {
                ViewBag.IsMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
                inputs.IsMobile = ViewBag.IsMobile;
                TopCarsByBodyTypeDto DTO = _topCarByBodyTypeAdapter.Get<TopCarsByBodyTypeDto, NewCarSearchInputs>(inputs);
                var pageNo = nvc["pageNo"];
                var pageSize = nvc["pageSize"];

                if (pageNo != null && pageSize != null && DTO.TotalModels > Convert.ToInt32(nvc["pageSize"]) * Convert.ToInt32(pageNo))
                {
                    DTO.NextPageUrl = string.Format("/topcars/filtered/?{0}", getQueryString(nvc));
                }
                DTO.IsFilterApplied = true;
                DTO.FilterAppliedCount = getFilterAppliedCount(nvc);
                DTO.CityName = (CookiesCustomers.MasterCityId <= 0) ? string.Empty : CookiesCustomers.MasterCity;
                DTO.CityZone = (CookiesCustomers.MasterZoneId <= 0) ? (CookiesCustomers.MasterCityId <= 0 ? "No City" : CookiesCustomers.MasterCity) : CookiesCustomers.MasterZone;
                return PartialView("~/Views/Shared/NewCars/_TopCarsList.cshtml", DTO);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "TopCarsByBodyTypeController.TopCars()\n Exception : " + ex.Message);
            }
            return new EmptyResult();
        }

        private static void SetPageProperties(TopCarsByBodyTypeDto topCarsByBodyType, CarBodyStyle bodyStyleId, short carBodyType)
        {

            topCarsByBodyType.BodyType = string.Format("{0}s", carBodyType > 0 ? bodyStyleId.ToFriendlyString() : CarBodyStyle.Hatchback.ToFriendlyString());
            topCarsByBodyType.BodyStyleId = bodyStyleId;
            topCarsByBodyType.CityName = (CookiesCustomers.MasterCityId <= 0) ? string.Empty : CookiesCustomers.MasterCity;
            topCarsByBodyType.CityZone = (CookiesCustomers.MasterZoneId <= 0) ? (CookiesCustomers.MasterCityId <= 0 ? "No City" : CookiesCustomers.MasterCity) : CookiesCustomers.MasterZone;
            topCarsByBodyType.DomainName = CWConfiguration.AdDomainName;
        }

        private string getQueryString(NameValueCollection nvc)
        {
            string queryString = null;
            foreach (var key in nvc.AllKeys)
            {
                queryString = String.Format("{0}{1}={2}", (String.IsNullOrWhiteSpace(queryString) ? queryString : string.Format("{0}&", queryString)), key, ((key == "pageNo") ? (Convert.ToInt32(nvc[key]) + 1).ToString() : nvc[key]));
            }
            return queryString;
        }

        private int getFilterAppliedCount(NameValueCollection nvc)
        {
            int count = 0;
            foreach (var key in nvc.AllKeys)
            {
                if (key != "pageNo" && key != "pageSize" && key != "cityId")
                {
                    if (!String.IsNullOrEmpty(nvc[key]))
                    {
                        count = count + nvc[key].Split(',').Length;
                    }
                }
            }
            return count;
        }
    }
}
