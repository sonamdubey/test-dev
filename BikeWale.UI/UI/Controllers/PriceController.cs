using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models;
using Bikewale.Models.Price;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd March 2017
    /// Description : To manage methods and pages related to price and quotation
    /// Modified by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Added IAdSlot.
    /// </summary>
    public class PriceController : Controller
    {

        private readonly IDealerPriceQuoteDetail _objDealerPQDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IPriceQuoteCache _objPQCache;
        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Resolve unity containers
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added IAdSlot.
        /// </summary>
        /// <param name="objDealerPQDetails"></param>
        /// <param name="objDealerPQ"></param>
        /// <param name="objVersionCache"></param>
        /// <param name="objAreaCache"></param>
        /// <param name="objCityCache"></param>
        /// <param name="objPQ"></param>
        /// <param name="objDealerCache"></param>
        public PriceController(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersions<BikeVersionEntity, uint> objVersion,
            IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IManufacturerCampaign objManufacturerCampaign, IAdSlot adSlot, IPriceQuoteCache objPQCache)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersion = objVersion;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _adSlot = adSlot;
            _objPQCache = objPQCache;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Index method to call pricequote default page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Controller method to call desktop dealerpricequote page with mpq 
        /// Modified by :   Sumit Kate on 19 May 2017
        /// Description :   Pass the lead source id
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in DealerPriceQuotePage object creation.
        /// </summary>
        /// <returns></returns>
        [Route("pricequote/dealer/")]
        public ActionResult Details()
        {
            DealerPriceQuotePage obj = new DealerPriceQuotePage(_objDealerPQDetails, _objDealerPQ, _objVersion, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objManufacturerCampaign, _adSlot, _objPQCache);
            obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.DPQ_Desktop;
            obj.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Desktop_DealerPriceQuote;
            obj.CurrentPageUrl = Request.RawUrl;
            obj.Platform = DTO.PriceQuote.PQSources.Desktop;
            obj.PQSource = PQSourceEnum.Desktop_DPQ_Quotation;
            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.OtherTopCount = 3;
                DealerPriceQuotePageVM objData = obj.GetData();
                //if data is null check for pricequote redirection
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
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Controller method to call mobile dealerpricequote page with mpq 
        /// Modified by :   Sumit Kate on 19 May 2017
        /// Description :   Pass the lead source id
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in DealerPriceQuotePage object creation.
        /// </summary>
        /// <returns></returns>
        [Route("m/pricequote/dealer/")]
        public ActionResult Details_Mobile()
        {
            DealerPriceQuotePage obj = new DealerPriceQuotePage(_objDealerPQDetails, _objDealerPQ, _objVersion, _objAreaCache, _objCityCache, _objPQ, _objDealerCache, _objManufacturerCampaign, _adSlot, _objPQCache);
            obj.LeadSource = Entities.BikeBooking.LeadSourceEnum.DPQ_Mobile;
            obj.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_DealerPriceQuote;
            obj.CurrentPageUrl = Request.RawUrl;
            obj.Platform = DTO.PriceQuote.PQSources.Mobile;
            obj.PQSource = PQSourceEnum.Mobile_DPQ_Quotation;
            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.OtherTopCount = 9;
                DealerPriceQuotePageVM objData = obj.GetData();
                //if data is null check for pricequote redirection
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
                return HttpNotFound();
            }
        }
    }
}