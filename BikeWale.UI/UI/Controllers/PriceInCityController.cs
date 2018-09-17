using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models;
using Bikewale.Models.PriceInCity;
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
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IBikeModels<Entities.BikeData.BikeModelEntity, int> _objModelEntity = null;
        private readonly IAdSlot _adSlot = null;
        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Constructor to intialize the member variables
        /// Modified by : Ashutosh Sharma on 11 Oct 2017
        /// Description : Added IBikeModels<Entities.BikeData.BikeModelEntity, int> instance in constructor for image gallery.
        /// Modifed by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Added IAdSlot.
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
        public PriceInCityController(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenterCache, IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeInfo bikeInfo, IBikeModelsCacheRepository<int> modelCache, IDealerPriceQuoteDetail objDealerDetails, IDealerPriceQuote objDealerPQ, ICityCacheRepository objCityCache, IAreaCacheRepository objAreaCache, IManufacturerCampaign objManufacturerCampaign, IBikeModels<Entities.BikeData.BikeModelEntity, int> modelEntity, IAdSlot adSlot)
        {
            _cityMaskingCache = cityMaskingCache;
            _modelMaskingCache = modelMaskingCache;
            _objPQ = objPQ;
            _objPQCache = objPQCache;
            _objDealerCache = objDealerCache;
            _objServiceCenterCache = objServiceCenterCache;
            _objVersion = objVersion;
            _bikeInfo = bikeInfo;
            _modelCache = modelCache;
            _objDealerDetails = objDealerDetails;
            _objDealerPQ = objDealerPQ;
            _objCityCache = objCityCache;
            _objAreaCache = objAreaCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _objModelEntity = modelEntity;
            _adSlot = adSlot;

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Model Price in city dekstop view action method
        /// Modified by : Ashutosh Sharma on 11 Oct 2017
        /// Description : Added _objModelEntity parameter in PriceInCityPage object creation.
        /// Modifed by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Added _adSlot parameter in PriceInCityPage object creation.
        /// Modified by : Rajan Chauhan on 09 Feb 2017
        /// Description : Changed NearestCityCount to 9
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("make/{makeName}/model/{modelName}/pricein/{cityName}/")]
        public ActionResult Index(string makeName, string modelName, string cityName)
        {
            PriceInCityPageVM objVM = null;
            PriceInCityPage model = new PriceInCityPage(_cityMaskingCache, _modelMaskingCache, _objPQ, _objPQCache, _objDealerCache, _objServiceCenterCache, _objVersion, _bikeInfo, _modelCache, _objDealerDetails, _objDealerPQ, _objCityCache, _objAreaCache, _objManufacturerCampaign, PQSourceEnum.Desktop_PriceInCity_Alternative, modelName, cityName, _objModelEntity, _adSlot, makeName);
            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BikeInfoTabCount = 4;
                model.NearestCityCount = 9;
                model.TopCount = 3;
                model.PQSource = PQSourceEnum.Desktop_PriceInCity;
                model.Platform = DTO.PriceQuote.PQSources.Desktop;
                model.LeadSource = Entities.BikeBooking.LeadSourceEnum.DPQ_Desktop;
                model.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Desktop_PriceInCity;
                model.CurrentPageUrl = Request.RawUrl;
                objVM = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Model Price in city mobile view action method
        /// Modified by : Ashutosh Sharma on 11 Oct 2017
        /// Description : Added _objModelEntity parameter in PriceInCityPage object creation.
        /// Modifed by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Added _adSlot parameter in PriceInCityPage object creation.
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Route("m/make/{makeName}/model/{modelName}/pricein/{cityName}/")]
        public ActionResult Index_Mobile(string makeName, string modelName, string cityName)
        {
            PriceInCityPageVM objVM = null;
            PriceInCityPage model = new PriceInCityPage(_cityMaskingCache, _modelMaskingCache, _objPQ, _objPQCache, _objDealerCache, _objServiceCenterCache, _objVersion, _bikeInfo, _modelCache, _objDealerDetails, _objDealerPQ, _objCityCache, _objAreaCache, _objManufacturerCampaign, PQSourceEnum.Mobile_PriceInCity_AlternateBikes, modelName, cityName, _objModelEntity, _adSlot, makeName);
            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BikeInfoTabCount = 3;
                model.NearestCityCount = 8;
                model.IsMobile = true;
                model.TopCount = 9;
                model.PQSource = PQSourceEnum.Mobile_PriceInCity;
                model.Platform = DTO.PriceQuote.PQSources.Mobile;
                model.LeadSource = Entities.BikeBooking.LeadSourceEnum.DPQ_Mobile;
                model.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_PriceInCity;
                model.CurrentPageUrl = Request.RawUrl;
                objVM = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (model.Status == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 30th Aug 2017
        /// Summary: Action method for price in city amp page
        /// </summary>
        /// <returns></returns>
        [Route("m/make/{makeName}/model/{modelName}/pricein/{cityName}/amp/")]
        public ActionResult Index_Mobile_Amp(string makeName, string modelName, string cityName)
        {
            PriceInCityPageAMPVM objVM = null;
            PriceInCityPage model = new PriceInCityPage(_cityMaskingCache, _modelMaskingCache, _objPQ, _objPQCache, _objDealerCache, _objServiceCenterCache, _objVersion, _bikeInfo, _modelCache, _objDealerDetails, _objDealerPQ, _objCityCache, _objAreaCache, _objManufacturerCampaign, PQSourceEnum.Mobile_PriceInCity_AlternateBikes, modelName, cityName, _objModelEntity, makeName);
            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.IsMobile = true;
                model.BikeInfoTabCount = 3;
                model.NearestCityCount = 8;
                model.TopCount = 9;
                model.PQSource = PQSourceEnum.Mobile_PriceInCity_Dealer_Detail_Click;
                model.Platform = DTO.PriceQuote.PQSources.Amp;
                model.LeadSource = Entities.BikeBooking.LeadSourceEnum.DPQ_Mobile;
                model.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_PriceInCity;
                model.CurrentPageUrl = Request.RawUrl.Substring(0, Request.RawUrl.LastIndexOf("amp/"));
                objVM = model.GetDataAMP();


                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (model.Status == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}