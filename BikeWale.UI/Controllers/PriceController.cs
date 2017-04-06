using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Models;
using Bikewale.Models.Price;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd March 2017
    /// Description : To manage methods and pages related to price and quotation
    /// </summary>
    public class PriceController : Controller
    {

        private readonly IDealerPriceQuoteDetail _objDealerPQDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerCacheRepository _objDealerCache = null;

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Resolve unity containers
        /// </summary>
        /// <param name="objDealerPQDetails"></param>
        /// <param name="objDealerPQ"></param>
        /// <param name="objVersionCache"></param>
        /// <param name="objAreaCache"></param>
        /// <param name="objCityCache"></param>
        /// <param name="objPQ"></param>
        /// <param name="objDealerCache"></param>
        public PriceController(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersionCache = objVersionCache;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;


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
        /// </summary>
        /// <returns></returns>
        [Route("pricequote/dealer/")]
        public ActionResult Details()
        {
            DealerPriceQuotePage obj = new DealerPriceQuotePage(_objDealerPQDetails, _objDealerPQ, _objVersionCache, _objAreaCache, _objCityCache, _objPQ, _objDealerCache);


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
                return Redirect("/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd March 2017
        /// Description  : Controller method to call mobile dealerpricequote page with mpq 
        /// </summary>
        /// <returns></returns>
        [Route("m/pricequote/dealer/")]
        public ActionResult Details_Mobile()
        {
            DealerPriceQuotePage obj = new DealerPriceQuotePage(_objDealerPQDetails, _objDealerPQ, _objVersionCache, _objAreaCache, _objCityCache, _objPQ, _objDealerCache);

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
                return Redirect("/pagenotfound.aspx");
            }
        }
    }
}