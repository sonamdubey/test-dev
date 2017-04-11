using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   PriceInCity Controller. It faciliates the Price in city section pages
    /// </summary>
    public class PriceInCityController : Controller
    {
        private readonly ICityMaskingCacheRepository _cityMaskingCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objServiceCenterCache = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Constructor to intialize the member variables
        /// </summary>
        /// <param name="cityMaskingCache"></param>
        /// <param name="modelMaskingCache"></param>
        /// <param name="objPQ"></param>
        /// <param name="objPQCache"></param>
        /// <param name="objDealerCache"></param>
        /// <param name="objServiceCenterCache"></param>
        /// <param name="versionCache"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="cityCache"></param>
        /// <param name="modelCache"></param>
        public PriceInCityController(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenterCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IBikeModelsCacheRepository<int> modelCache, IDealerPriceQuoteDetail objDealerDetails, IDealerPriceQuote objDealerPQ, ICityCacheRepository objCityCache, IAreaCacheRepository objAreaCache)
        {
            _cityMaskingCache = cityMaskingCache;
            _modelMaskingCache = modelMaskingCache;
            _objPQ = objPQ;
            _objPQCache = objPQCache;
            _objDealerCache = objDealerCache;
            _objServiceCenterCache = objServiceCenterCache;
            _versionCache = versionCache;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _modelCache = modelCache;
            _objDealerDetails = objDealerDetails;
            _objDealerPQ = objDealerPQ;
            _objCityCache = objCityCache;
            _objAreaCache = objAreaCache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Model Price in city dekstop view action method
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("model/{modelName}/pricein/{cityName}/")]
        public ActionResult Index(string modelName, string cityName)
        {
            PriceInCityPageVM objVM = new PriceInCityPageVM();
            PriceInCityPage model = new PriceInCityPage(_cityMaskingCache, _modelMaskingCache, _objPQ, _objPQCache, _objDealerCache, _objServiceCenterCache, _versionCache, _bikeInfo, _cityCache, _modelCache, _objDealerDetails, _objDealerPQ, _objCityCache, _objAreaCache, PQSourceEnum.Desktop_PriceInCity_Alternative, modelName, cityName);
            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BikeInfoTabCount = 4;
                model.NearestCityCount = 8;
                model.TopCount = 3;
                model.PQSource = PQSourceEnum.Desktop_PriceInCity_Dealer_Detail_Click;
                model.Platform = DTO.PriceQuote.PQSources.Desktop;
                objVM = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (model.Status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Model Price in city mobile view action method
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Route("m/model/{modelName}/pricein/{cityName}/")]
        public ActionResult Index_Mobile(string modelName, string cityName)
        {
            PriceInCityPageVM objVM = new PriceInCityPageVM();
            PriceInCityPage model = new PriceInCityPage(_cityMaskingCache, _modelMaskingCache, _objPQ, _objPQCache, _objDealerCache, _objServiceCenterCache, _versionCache, _bikeInfo, _cityCache, _modelCache, _objDealerDetails, _objDealerPQ, _objCityCache, _objAreaCache, PQSourceEnum.Mobile_PriceInCity_AlternateBikes, modelName, cityName);
            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BikeInfoTabCount = 3;
                model.NearestCityCount = 4;
                model.TopCount = 9;
                model.PQSource = PQSourceEnum.Mobile_PriceInCity_Dealer_Detail_Click;
                model.Platform = DTO.PriceQuote.PQSources.Mobile;
                objVM = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (model.Status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }
    }
}