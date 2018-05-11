using System.Web.Mvc;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.BikeModels;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Added IAdSlot.
    /// </summary>
    public class ModelController : Controller
    {

        private readonly IBikeModels<Entities.BikeData.BikeModelEntity, int> _objModel = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IVideos _objVideos = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedBikescache = null;
        private readonly IServiceCenter _objServiceCenter;
        private readonly IPriceQuoteCache _objPQCache;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikesCache _usedBikesCache;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository;
		private readonly IApiGatewayCaller _apiGatewayCaller;

		/// <summary>
		/// Modified by : Ashutosh Sharma on 31 Oct 2017
		/// Description : Added IAdSlot.
        /// Modified by : Rajan Chauhan on 17 Apr 2018
        /// Description : Removed IBikeModelsCacheRepository dependency
		/// </summary>
		public ModelController(IUserReviewsCache userReviewsCache, IUserReviewsSearch userReviewsSearch, IBikeModels<Entities.BikeData.BikeModelEntity, int> objModel, IDealerPriceQuote objDealerPQ, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IDealerPriceQuoteDetail objDealerDetails, IBikeVersions<BikeVersionEntity, uint> objVersion, ICMSCacheContent objArticles, IVideos objVideos, IUsedBikeDetailsCacheRepository objUsedBikescache, IServiceCenter objServiceCenter, IPriceQuoteCache objPQCache, IUserReviewsCache userReviewCache, IUsedBikesCache usedBikesCache, IBikeModelsCacheRepository<int> objBestBikes, IUpcoming upcoming, IManufacturerCampaign objManufacturerCampaign, IBikeSeries bikeSeries, IAdSlot adSlot, IBikeInfo bikeInfo, IBikeMakesCacheRepository bikeMakesCacheRepository, IApiGatewayCaller apiGatewayCaller)
        {
            _objModel = objModel;
            _objDealerPQ = objDealerPQ;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _upcoming = upcoming;
            _objDealerCache = objDealerCache;
            _objDealerDetails = objDealerDetails;
            _objVersion = objVersion;
            _objArticles = objArticles;
            _objVideos = objVideos;
            _objUsedBikescache = objUsedBikescache;
            _objServiceCenter = objServiceCenter;
            _objPQCache = objPQCache;
            _usedBikesCache = usedBikesCache;
            _userReviewsSearch = userReviewsSearch;
            _userReviewsCache = userReviewsCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _bikeSeries = bikeSeries;
            _adSlot = adSlot;
            _bikeInfo = bikeInfo;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
			_apiGatewayCaller = apiGatewayCaller;

		}
        /// <summary>
        /// Modified by :- Subodh Jain on 17 july 2017
        /// Summary added _userReviewsSearch, _userReviewsCache
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in ModelPage object creation.
        /// Modified by :   Ashish Kamble on 22 Nov 2017
        /// Description :   Added Model Page view path.
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        // GET: Models
        [Route("model/{makeMasking}-bikes/{modelMasking}/"), Filters.DeviceDetection]
        public ActionResult Index(string makeMasking, string modelMasking, uint? versionId)
        {
            ModelPage obj = new ModelPage(makeMasking, modelMasking, _userReviewsSearch, _userReviewsCache, _objModel, _objDealerPQ, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objDealerDetails, _objVersion, _objArticles, _objVideos, _objUsedBikescache, _objServiceCenter, _objPQCache, _usedBikesCache, _upcoming, _objManufacturerCampaign, _bikeSeries, _adSlot, _bikeInfo, _bikeMakesCacheRepository, _apiGatewayCaller);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.Source = DTO.PriceQuote.PQSources.Desktop;
                obj.PQSource = Entities.PriceQuote.PQSourceEnum.Desktop_ModelPage;
                obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.Model_Desktop;
                obj.ManufacturerCampaignPageId = ManufacturerCampaignServingPages.Desktop_Model_Page;
                obj.CurrentPageUrl = Request.RawUrl;
                ModelPageVM objData = obj.GetData(versionId);
                //if data is null check for new bikes page redirection
                if (obj.Status.Equals(StatusCodes.RedirectPermanent))
                {
                    return RedirectPermanent(obj.RedirectUrl);
                }
                else return View("~/views/model/Index.cshtml", objData);// Do not remove View path from here.

            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }
        /// <summary>
        /// Modified by :- Subodh Jain on 17 july 2017
        /// Summary added _userReviewsSearch, _userReviewsCache
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in ModelPage object creation.
        /// Modified by :   Ashish Kamble on 22 Nov 2017
        /// Description :   Added Model Page view path.
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [Route("m/model/{makeMasking}-bikes/{modelMasking}/")]
        public ActionResult Index_Mobile(string makeMasking, string modelMasking, uint? versionId)
        {
            ModelPage obj = new ModelPage(makeMasking, modelMasking, _userReviewsSearch, _userReviewsCache, _objModel, _objDealerPQ, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objDealerDetails, _objVersion, _objArticles, _objVideos, _objUsedBikescache, _objServiceCenter, _objPQCache, _usedBikesCache, _upcoming, _objManufacturerCampaign, _bikeSeries, _adSlot, _bikeInfo, _bikeMakesCacheRepository, _apiGatewayCaller);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.IsMobile = true;
                obj.Source = DTO.PriceQuote.PQSources.Mobile;
                obj.PQSource = Entities.PriceQuote.PQSourceEnum.Mobile_ModelPage;
                obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.Model_Mobile;
                obj.ManufacturerCampaignPageId = ManufacturerCampaignServingPages.Mobile_Model_Page;
                obj.CurrentPageUrl = Request.RawUrl;
                ModelPageVM objData = obj.GetData(versionId);
                //if data is null check for new bikes page redirection
                if (obj.Status.Equals(StatusCodes.RedirectPermanent))
                {
                    return RedirectPermanent(obj.RedirectUrl);
                }
                else return View("~/views/model/Index_mobile.cshtml", objData); // Do not remove View path from here.

            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }


    }   // class
}   // namespace