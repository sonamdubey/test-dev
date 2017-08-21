using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerBikePrice;
using BikewaleOpr.Models.DealerPricing;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Controller for Dealer Pricing Management page.
    /// </summary>
    [Authorize]
    public class DealerBikePriceController : Controller
    {
        private readonly ILocation location = null;
        private readonly IDealerPriceQuote dealerPriceQuote = null;
        private readonly IDealerPrice dealerPrice = null;
        private readonly IDealers dealersRepository = null;
        public DealerBikePriceController(
            ILocation locationObject, IDealerPriceQuote dealerPriceQuoteObject,
            IDealerPrice dealerPriceObject, IDealers dealersRepositoryObject)
        {
            location = locationObject;
            dealerPriceQuote = dealerPriceQuoteObject;
            dealerPrice = dealerPriceObject;
            dealersRepository = dealersRepositoryObject;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Performs UI binding for Index page
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("dealers/operations/")]
        public ActionResult Index()
        {
            DealerPricingIndexPageModel DP_IndexPageModel = new DealerPricingIndexPageModel(location, dealerPriceQuote);
            return View(DP_IndexPageModel.GetLandingInformation());
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Performs UI binding for price sheet page
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <param name="otherCityId"></param>
        /// <param name="dealerName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [HttpGet, Route("dealers/{dealerId}/dealercity/{cityId}/brand/{makeId}/pricing/")]
        public ActionResult DealerPricing(uint cityId, uint makeId, uint dealerId, uint? otherCityId, string dealerName, string cityName)
        {
            DealerPricingSheetPageModel DPSheetPageModel = new DealerPricingSheetPageModel(location, dealerPriceQuote, dealerPrice, dealersRepository);
            DealerPricingSheetPageVM DPSheetPageVM = null;

            if (otherCityId.HasValue && otherCityId.Value > 0)
                DPSheetPageVM = DPSheetPageModel.GetPriceSheetInformation(cityId, makeId, dealerId, otherCityId.Value, dealerName, cityName);
            else
                DPSheetPageVM = DPSheetPageModel.GetPriceSheetInformation(cityId, makeId, dealerId, dealerName, cityName);

            return View(DPSheetPageVM);
        }
    }
}