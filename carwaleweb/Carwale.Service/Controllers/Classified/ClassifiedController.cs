using Carwale.BL.Interface.Stock.Search;
using Carwale.DTOs.Classified;
using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Classified.Stock.Ios;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Elastic;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers
{
    public partial class ClassifiedController : ApiController
    {
        private readonly IElasticSearchManager _searchManager;
        private readonly IStockRepository _stockRepository;
        private readonly ICarDetailsCache _carDetailsCache;
        private readonly IStockSearchLogic<SearchResultDesktop> _stockSearchLogic;
        private static readonly string elasticIndexName = ConfigurationManager.AppSettings["ElasticIndexName"];
        private const int maxPage10 = 10;
        private const int maxPage20 = 20;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public ClassifiedController(IStockRepository stockRepository, IElasticSearchManager searchManager, ICarDetailsCache carDetailsCache, IStockSearchLogic<SearchResultDesktop> stockSearchLogic)
        {
            _stockRepository = stockRepository;
            _searchManager = searchManager;
            _carDetailsCache = carDetailsCache;
            _stockSearchLogic = stockSearchLogic;
        }
        /// <summary>
        /// Get Results data with pager and filters count || Added by Jugal
        /// Modified By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get search result from elastic search index
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ActionName("stockfilters")]
        [PlatformRequired]
        public IHttpActionResult GetResultsWithFiltersAndPager([FromUri] FilterInputs filterInputs)
        {
            IHttpActionResult result = null;
            try
            {
                var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

                if (requestSource == (int)Platform.CarwaleDesktop)
                {
                    result = CreateStockResponse<SearchResultDesktop>(_searchManager, filterInputs, maxPage10);
                }
                else
                {
                    return BadRequest("Source Id is incorrect");
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetResultsWithFiltersAndPager()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return result;
        }

        [HttpGet]
        [ActionName("stockRecommendations")]
        [PlatformRequired]
        public IHttpActionResult GetRecommendations([FromUri] FilterInputs filterInputs)
        {
            ResultsRecommendation responseRecommendations = null;
            try
            {
                var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
                if (requestSource == (int)Platform.CarwaleDesktop || requestSource == (int)Platform.CarwaleMobile || requestSource == (int)Platform.CarwaleAndroid || requestSource == (int)Platform.CarwaleiOS)
                {
                    responseRecommendations = _searchManager.SearchIndex<ResultsRecommendation>(elasticIndexName, filterInputs);
                }
                else
                {
                    return BadRequest("Source Id is incorrect");
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendations()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(responseRecommendations);
        }

        [HttpGet]
        [Route("webapi/profileRecommendations/"), EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetRecommendationsForProfile(string profileId)
        {
            List<StockBaseEntity> responseRecommendations = null;
            try
            {
                responseRecommendations = _searchManager.SearchIndexProfileRecommendation<List<StockBaseEntity>, string>(profileId.ToUpper(), Constants.WebAndMobileRecommendationCount);
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendationsForProfile()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(responseRecommendations);
        }

        /// <summary>
        /// API for Only Results data || Added by Jugal 
        /// Modified By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get search result from elastic search index
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [ActionName("stock")]
        [PlatformRequired]
        public IHttpActionResult GetSearchResults([FromUri] FilterInputs filterInputs)
        {
            IHttpActionResult result = null;

            try
            {
                var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

                switch (requestSource)
                {
                    case (int)Platform.CarwaleMobile:
                        result = CreateStockResponse<StockResultsMobile>(_searchManager, filterInputs, maxPage20);
                        break;
                    case (int)Platform.CarwaleAndroid:
                        result = CreateStockResponse<StockResultsAndroid>(_searchManager, filterInputs, maxPage10);
                        break;
                    case (int)Platform.CarwaleiOS:
                        result = CreateStockResponse<StockResultIos>(_searchManager, filterInputs, maxPage10);
                        break;
                    default:
                        return BadRequest("Source Id is incorrect");
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetSearchResults()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return result;
        }

        /// <summary>
        /// API for only Filters Count || Added by Jugal
        /// Modified By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get search result from elastic search index
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ActionName("filters")]
        [PlatformRequired]
        public IHttpActionResult GetFiltersCount([FromUri] FilterInputs filterInputs)
        {
            IHttpActionResult result = null;

            try
            {
                var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

                switch (requestSource)
                {
                    case (int)Platform.CarwaleAndroid:
                        result = CreateStockResponse<FilterCountsAndroid>(_searchManager, filterInputs);
                        break;
                    case (int)Platform.CarwaleiOS:
                        result = CreateStockResponse<FilterCountIos>(_searchManager, filterInputs);
                        break;
                    default:
                        return BadRequest("Source Id is incorrect");
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetFiltersCount()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return result;
        }

        /// <summary>
        /// Call the elastic search manager for fetching the Total Stock count for selected filters
        /// Author: Navead Kazi
        /// Date : 11/12/2015
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <returns>HttpResponseMessage with total count</returns>
        [HttpGet]
        [ActionName("totalcount")]
        [PlatformRequired]
        public IHttpActionResult GetTotalCount([FromUri] FilterInputs filterInputs)
        {
            JObject resp = null;
            try
            {
                var requestSource = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
                if (requestSource == (int)Platform.CarwaleDesktop || requestSource == (int)Platform.CarwaleMobile || requestSource == (int)Platform.CarwaleAndroid || requestSource == (int)Platform.CarwaleiOS)
                {
                    int totalStockCount = _searchManager.GetTotalStockCount(ConfigurationManager.AppSettings["ElasticIndexName"], filterInputs);
                    resp = JObject.FromObject(new { totalCount = totalStockCount });
                }
                else
                {
                    return BadRequest("Source Id is incorrect");
                }

            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetTotalCount()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(resp);
        }

        public IHttpActionResult GetProfileImageDetails()
        {
            ImageGalleryEntity getImages = null;

            try
            {
                string inquiryId = HttpContext.Current.Request.QueryString["inquiryId"].ToString();
                bool isDealer = Convert.ToBoolean(HttpContext.Current.Request.QueryString["isDealer"]);
                getImages = _stockRepository.GetImagesByProfileId(inquiryId, isDealer);
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetProfileImageDetails()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetProfileImageDetails()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(getImages);
        }

        private IHttpActionResult CreateStockResponse<T>(IElasticSearchManager searchManager, FilterInputs filterInputs, int maxPage = 0)
        {
            T results;

            if (maxPage > 0 && !string.IsNullOrEmpty(filterInputs.pn) && Convert.ToInt32(filterInputs.pn) > maxPage)
            {
                return Ok();
            }
            else
            {
                if(typeof(T) == typeof(SearchResultDesktop))
                {
                    results = (T)Convert.ChangeType(_stockSearchLogic.Get(filterInputs), typeof(SearchResultDesktop));
                }
                else
                {
                    results = searchManager.SearchIndex<T>(elasticIndexName, filterInputs);
                }
                
            }
            if (typeof(T) == typeof(StockResultIos))
            {
                return Json(results, _serializerSettings);
            }
            else
            {
                return Ok(results);
            }
        }
    } //class
} //namespace
