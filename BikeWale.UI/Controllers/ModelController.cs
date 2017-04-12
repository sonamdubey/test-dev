using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.BikeModels;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class ModelController : Controller
    {

        private readonly IBikeModels<BikeModelEntity, int> _objModel = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IVideos _objVideos = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedBikescache = null;
        private readonly IServiceCenter _objServiceCenter;
        private readonly IPriceQuoteCache _objPQCache;
        private readonly IBikeCompareCacheRepository _objCompare;
        private readonly IUserReviewsCache _userReviewCache;
        private readonly IUsedBikesCache _usedBikesCache;
        public ModelController(IBikeModels<BikeModelEntity, int> objModel, IDealerPriceQuote objDealerPQ, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IDealerPriceQuoteDetail objDealerDetails, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, ICMSCacheContent objArticles, IVideos objVideos, IUsedBikeDetailsCacheRepository objUsedBikescache, IServiceCenter objServiceCenter, IPriceQuoteCache objPQCache, IBikeCompareCacheRepository objCompare, IUserReviewsCache userReviewCache, IUsedBikesCache usedBikesCache, IBikeModelsCacheRepository<int> objBestBikes)
        {
            _objModel = objModel;
            _objDealerPQ = objDealerPQ;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objDealerDetails = objDealerDetails;
            _objVersionCache = objVersionCache;
            _objArticles = objArticles;
            _objVideos = objVideos;
            _objUsedBikescache = objUsedBikescache;
            _objServiceCenter = objServiceCenter;
            _objPQCache = objPQCache;
            _objCompare = objCompare;
            _userReviewCache = userReviewCache;
            _usedBikesCache = usedBikesCache;
            _objBestBikes = objBestBikes;
        }

        // GET: Models
        [Route("model/{makeMasking}-bikes/{modelMasking}/"), Filters.DeviceDetection]
        public ActionResult Index(string makeMasking, string modelMasking, uint? versionId)
        {
            ModelPage obj = new ModelPage(makeMasking, modelMasking, _objModel, _objDealerPQ, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objDealerDetails, _objVersionCache, _objArticles, _objVideos, _objUsedBikescache, _objServiceCenter, _objPQCache, _objCompare, _userReviewCache, _usedBikesCache, _objBestBikes);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.Source = DTO.PriceQuote.PQSources.Desktop;
                obj.PQSource = Entities.PriceQuote.PQSourceEnum.Desktop_ModelPage;
                obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.Model_Desktop;
                ModelPageVM objData = obj.GetData(versionId);
                //if data is null check for new bikes page redirection
                if (obj.Status.Equals(StatusCodes.RedirectPermanent))
                {
                    return RedirectPermanent(obj.RedirectUrl);
                }
                else return View(objData);

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

        [Route("m/model/{makeMasking}-bikes/{modelMasking}/")]
        public ActionResult Index_Mobile(string makeMasking, string modelMasking, uint? versionId)
        {
            ModelPage obj = new ModelPage(makeMasking, modelMasking, _objModel, _objDealerPQ, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objDealerDetails, _objVersionCache, _objArticles, _objVideos, _objUsedBikescache, _objServiceCenter, _objPQCache, _objCompare, _userReviewCache, _usedBikesCache, _objBestBikes);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.Source = DTO.PriceQuote.PQSources.Mobile;
                obj.PQSource = Entities.PriceQuote.PQSourceEnum.Mobile_ModelPage;
                obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.Model_Mobile;
                ModelPageVM objData = obj.GetData(versionId);
                //if data is null check for new bikes page redirection
                if (obj.Status.Equals(StatusCodes.RedirectPermanent))
                {
                    return RedirectPermanent(obj.RedirectUrl);
                }
                else return View(objData);

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