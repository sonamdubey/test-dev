using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Models;
using Bikewale.Models.Price;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PriceController : Controller
    {

        private readonly IDealerPriceQuoteDetail _objDealerPQDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IPriceQuote _objPQ = null;

        public PriceController(IDealerPriceQuoteDetail objDealerPQDetails, IDealerPriceQuote objDealerPQ, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ)
        {
            _objDealerPQDetails = objDealerPQDetails;
            _objDealerPQ = objDealerPQ;
            _objVersionCache = objVersionCache;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("pricequote/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("pricequote/dealer/")]
        public ActionResult Details()
        {
            DealerPriceQuotePage obj = new DealerPriceQuotePage(_objDealerPQDetails, _objDealerPQ, _objVersionCache, _objAreaCache, _objCityCache, _objPQ);

            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                DealerPriceQuotePageVM objData = obj.GetData();

                return View(objData);
            }
        }
    }
}