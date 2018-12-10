using Carwale.Interfaces.Deals;
using Carwale.Notifications;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Carwale.Utility;
using Carwale.Entity.Enum;
using Carwale.DTOs.Deals;
using Carwale.BL.Deals;
using Carwale.Service.Filters;
using Carwale.DTOs;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Entity.Deals;
using Newtonsoft.Json;
using AutoMapper;
using System.Net.Http;
using Carwale.Interfaces;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.CarData;
using Carwale.Entity;
using Carwale.DTOs.CarData;
using System.Web.Http.Cors;
using Carwale.Entity.CarData;

namespace Carwale.Service.Controllers.Deals
{
    [EnableCors(origins: "http://webserver:8082, http://oprst.carwale.com,http://opr.carwale.com,http://172.16.2.114:8081,http://localhost:8081,http://localhost:8082", headers: "*", methods: "*")]
    public class AdvantageServiceController : ApiController
    {
        private readonly IDeals _deals;
        private readonly IRepository<Cities> _geoCity;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepository;
        private readonly ICarVersions _carVersionBl;

        public AdvantageServiceController(IDeals deals, IRepository<Cities> geoCity, ICarVersionCacheRepository carVersionsCacheRepository, ICarVersions carVersionBl)
        {
            _deals = deals;
            _geoCity = geoCity;
            _carVersionsCacheRepository = carVersionsCacheRepository;
            _carVersionBl = carVersionBl;
        }

        [HttpGet, Route("api/deals/maxDiscount")]
        public IHttpActionResult GetDealsMaxDiscount(int modelId, int cityId)
        {
            var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
            try
            {
                if (requestSource == (int)Platform.CarwaleDesktop || requestSource == (int)Platform.CarwaleAndroid || requestSource == (int)Platform.CarwaleiOS || requestSource == (int)Platform.CarwaleMobile)
                {
                    var discountSummary = _deals.GetDealsSummaryByModelandCity_Android(modelId, cityId);
                    if (discountSummary != null)
                    {
                        discountSummary.VersionsDiscount = _deals.BestVersionDealsByModel(modelId, cityId);
                    }

                    return Ok(discountSummary);
                }
                else
                {
                    return Ok("Platform source was missing");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDealsMaxDiscount()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/advantage/stock/{stockId}")]
        [ValidateSourceFilter]
        public IHttpActionResult GetStockDetails(int stockId, int cityId)
        {
            try
            {
                var stockDetails = _deals.GetStockDetails(stockId, cityId);
                return Ok(stockDetails);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetStockDetails()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/deals/makemodel/")]
        public IHttpActionResult GetDealMakesByCity(int cityId)
        {
            try
            {
                var dealsMakes = _deals.GetDealMakesByCity(cityId);
                return Ok(dealsMakes);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDealMakesByCity()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/deals/makemodel/")]
        public IHttpActionResult GetDealModelsByMakeAndCity(int cityId, int makeId)
        {
            try
            {
                var dealsModels = _deals.GetDealModelsByMakeAndCity(cityId, makeId);
                return Ok(dealsModels);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDealModelsByMakeAndCity()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/deals/recommendedDeals/")]
        public IHttpActionResult GetRecommendedDeals(int recommendationCount, int cityId, int dealerId = 0, string currentModel = "", string models = "")
        {
            try
            {
                string userHistory = GetUserHistory();
                var dealsRecommendation = _deals.GetDealsRecommendation(models, recommendationCount, cityId, userHistory, dealerId, currentModel);
                return Ok(dealsRecommendation);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetRecommendedDeals()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/deals/")]
        public IHttpActionResult GetProductDetails(int modelId, int cityId, int versionId = 0)
        {
            try
            {
                return Ok(_deals.GetProductDetails(modelId, versionId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetProductDetails()");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        public string GetUserHistory()
        {
            string userModelHistory = "", userAdvHistory = "";
            string userHistorys;

            if (HttpContext.Current.Request.Cookies["_userModelHistory"] != null && HttpContext.Current.Request.Cookies["_userModelHistory"].Value.ToString() != "")
            {
                userModelHistory = HttpContext.Current.Request.Cookies["_userModelHistory"].Value.ToString();
            }

            if (HttpContext.Current.Request.Cookies["_advHistory"] != null && HttpContext.Current.Request.Cookies["_advHistory"].Value.ToString() != "")
            {
                userAdvHistory = HttpContext.Current.Request.Cookies["_advHistory"].Value.ToString();
            }

            userHistorys = MergeUserHistory(userAdvHistory, userModelHistory);

            if (!String.IsNullOrEmpty(userHistorys))
                return userHistorys.Replace('~', ',');
            else
                return userHistorys;
        }

        private string MergeUserHistory(string userAdvHistory, string userModelHistory)
        {
            if (String.IsNullOrEmpty(userAdvHistory) && String.IsNullOrEmpty(userModelHistory))
                return "";
            else if (String.IsNullOrEmpty(userAdvHistory))
                return userModelHistory;
            else if (String.IsNullOrEmpty(userModelHistory))
                return userAdvHistory;
            else
            {
                var modelHistory = userModelHistory.Split('~');
                var advHistory = userAdvHistory.Split('~');
                return string.Join("~", modelHistory.Union(advHistory));
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/cities/")]
        public IHttpActionResult GetAdvantageCities(int modelId = 0, int versionId = 0, int makeId = 0)
        {
            var cityDetails = _deals.GetAdvantageCities(modelId, versionId, makeId);
            return Ok(cityDetails);
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/getDeals/")]
        public IHttpActionResult GetDeals([FromUri] Carwale.Entity.Deals.Filters filter)
        {
            var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
            try
            {
                filter.CityId = filter.CityId == 0  ? 1 : filter.CityId;
                string[] BugetString = filter.budget != null && (filter.budget.IndexOf('-') >0) ? filter.budget.Split('-') : null;
                if (BugetString != null)
                {
                    filter.StartBudget = String.IsNullOrEmpty(BugetString[0]) ? 0 : (int)(Convert.ToDecimal(BugetString[0]) * 100000);
                    filter.EndBudget = String.IsNullOrEmpty(BugetString[1]) ? 0 : (int)(Convert.ToDecimal(BugetString[1]) * 100000);
                }
                AdvantageSearchResultsDTO advantageSearch = new AdvantageSearchResultsDTO();
                if (filter != null && filter.PS <= 0)
                {
                    int pageSize = 0;
                    if (requestSource == (int)Platform.CarwaleAndroid || requestSource == (int)Platform.CarwaleiOS)
                        Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["AdvantagePageSizeApp"].ToString(), out pageSize);
                    else
                        Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["AdvantagePageSizeDesktop"].ToString(), out pageSize);
                    filter.PS = pageSize;
                }
                advantageSearch = _deals.GetDeals(filter);

                if (requestSource == (int)Platform.CarwaleDesktop || requestSource == (int)Platform.CarwaleMobile)
                    advantageSearch.Deals.ForEach(x => x.SpecificationsOverview = String.Join(",", _carVersionBl.GetCarVersions(x.Model.ModelId, Status.All).Where(z => z.Id == x.Version.ID).Select(y => y.SpecsSummary).FirstOrDefault().Split(',').ToList().Take(2)));

                int modelId = Advanatge.GetOfferModelId(filter.CityId);
                if (modelId > 0)
                    advantageSearch.OfferOfTheWeek = _deals.GetOfferOfWeekDetails(modelId, filter.CityId);
                return Ok(advantageSearch);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDeals()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/savings")]
        public IHttpActionResult GetBestSavingDeals(int cityId)
        {
            try
            {
                int carCount = 15;
                Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["AdvantagePageSizeDesktop"].ToString(), out carCount);
                return Ok(_deals.GetDealsByDiscount(cityId, carCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDeals()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/recommmendations")]
        public IHttpActionResult GetRecommendations(int cityId, int recommendationCount, int modelId, string advHistory = "", string pqHistory = "")
        {
            try
            {
                List<DealsRecommendationDTO> advUserHistoryDeals;
                List<DealsRecommendationDTO> pqUserHistory;
                List<DealsRecommendationDTO> bestSavings;

                if (!String.IsNullOrWhiteSpace(advHistory))
                    advUserHistoryDeals = _deals.GetAdvUserHistoryDeals(advHistory, recommendationCount, cityId, modelId);
                else
                    advUserHistoryDeals = new List<DealsRecommendationDTO>();
                if (!String.IsNullOrWhiteSpace(pqHistory))
                {
                    pqUserHistory = _deals.GetPQUserHistoryDeals(advUserHistoryDeals, recommendationCount, _deals.RemoveCurrentModel(pqHistory, modelId.ToString()), cityId, modelId);
                }
                else
                    pqUserHistory = new List<DealsRecommendationDTO>();
                bestSavings = _deals.GetBestSavingsCar(recommendationCount + 1, modelId, cityId);
                bestSavings = bestSavings.Where(bs => bs.Savings > 0).ToList();
                return Ok(new { advUserHistory = advUserHistoryDeals, pqUserHistory, bestSavings });
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetRecommendations()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/similarDeals")]
        public IHttpActionResult GetSimilarDeals(int modelId, int cityId)
        {
            try
            {
                return Ok(_deals.GetSimilarDeals(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetSimilarDeals()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/v1/advantage/savings")]
        public IHttpActionResult GetBestSavingDealsApp([FromUri] Carwale.Entity.Deals.Filters filter)
        {
            try
            {
                AdvantageSearchResultsDTO advantageSearch = new AdvantageSearchResultsDTO();
                if (filter != null && filter.PS == 0)
                {
                    int pageSize = 0;
                    Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["AppCarCount"].ToString(), out pageSize);
                    filter.PS = pageSize;
                }
                filter.PN = 1;
                filter.SC = 0;
                filter.SO = 1;
                advantageSearch = _deals.GetDeals(filter);
                return Ok(advantageSearch.Deals);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetBestSavingDealsAndroid()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/pricebreakup")]
        public IHttpActionResult GetDealsPriceBreakup(int stockId, int cityId)
        {
            try
            {
                return Ok(_deals.GetDealsPriceBreakup(stockId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetDealsPriceBreakup()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [ValidateSourceFilter]
        [Route("api/advantage/presence")]
        public IHttpActionResult IsAdvantageCity(int cityId)
        {
            try
            {
                var isAdvantageCity = _deals.IsShowDeals(cityId, true);
                return Ok(new { isAdvantageCity });
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.IsAdvantageCity()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/xml/makeModelVersionColor")]
        public HttpResponseMessage GetMakeModelVersionColor()
        {
            return new HttpResponseMessage() { Content = new StringContent(_deals.GetMakeModelVersionColor(), Encoding.UTF8, "application/xml") };
        }

        [HttpGet, Route("api/xml/cities")]
        public HttpResponseMessage GetAllCities()
        {
            var data = _geoCity.GetAll();
            string dataXML = Format.XMLSerialize<IEnumerable<Cities>>(data);
            return new HttpResponseMessage() { Content = new StringContent(dataXML, Encoding.UTF8, "application/xml") };
        }

        
        [HttpGet]
        [Route("api/advantage/versions")]
        public IHttpActionResult GetAdvantageVersions(int modelId,int cityId)
        {
            try
            {
                return Ok<IEnumerable<CarVersionsDTO>>(_deals.GetAdvantageVersions(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetAdvantageVersions()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/advantage/version-colors")]
        public IHttpActionResult GetAdvantageVersionColors(int versionId, int cityId)
        {
            try
            {
                return Ok<IEnumerable<CarColorDTO>>(_deals.GetAdvantageVersionColors(versionId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetAdvantageVersionColors()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/advantage/version-color-year")]
        public IHttpActionResult GetAdvantageColorYears(int versionId, int colorId, int cityId)
        {
            try
            {
                return Ok<IEnumerable<int>>(_deals.GetAdvantageColorYears(versionId, colorId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetAdvantageColorYears()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/deals/offers")]
        public IHttpActionResult GetOfferList(int stockId, int cityId)
        {
            var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
            try
            {
                if (requestSource == (int)Platform.CarwaleDesktop || requestSource == (int)Platform.CarwaleAndroid || requestSource == (int)Platform.CarwaleiOS || requestSource == (int)Platform.CarwaleMobile)
                {
                    return Ok<List<KeyValuePair<string, string>>>(_deals.FillOfferList(_deals.GetOffers(stockId.ToString(), cityId), stockId));
                }
                else
                {
                    return Ok("Platform source was missing");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetOfferTypes()");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/deals/filtered-versions"), ValidateSourceFilter]
        public IHttpActionResult GetFilteredVersionDeals([FromUri] Entity.Deals.Filters filter, int modelId, int versionId)
        {
            var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

            if (filter.Fuels != null)
            filter.Fuels = filter.Fuels.Replace(" ", ",");
            if (filter.Transmissions != null)
            filter.Transmissions = filter.Transmissions.Replace(" ", ",");
            if (filter.BodyTypes != null)
            filter.BodyTypes = filter.BodyTypes.Replace(" ", ",");
            List<CarVersions> summaryList = _carVersionBl.GetCarVersions(modelId, Status.All);
            try
            {
                var deals = _deals.GetFilteredVersionDeals(modelId, versionId, filter);
                if (deals != null)
                    deals.ForEach(x => x.SpecificationsOverview = String.Join(",", summaryList.Where(z => z.Id == x.Version.ID).Select(y => y.SpecsSummary).FirstOrDefault().Split(',').ToList().Take(2)));
                    return Ok(deals);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealsController.GetBestSavingDealsAndroid()");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
