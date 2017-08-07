using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerBikePrice;
using BikewaleOpr.Models.DealerPricing;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
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

        public ActionResult Index()
        {
            DealerPricingIndexPageModel DP_IndexPageModel = new DealerPricingIndexPageModel(location, dealerPriceQuote);
            return View(DP_IndexPageModel.GetLandingInformation());
        }

        [HttpGet, Route("dealerbikepricing/{dealerId}/dealercity/{cityId}/brand/{makeId}/")]
        public ActionResult GetDealerPricing(uint cityId, uint makeId, uint dealerId, uint? otherCityId)
        {
            DealerPricingSheetPageModel DPSheetPageModel = new DealerPricingSheetPageModel(location, dealerPriceQuote, dealerPrice, dealersRepository);
            DealerPricingSheetPageVM DPSheetPageVM = null;

            if (otherCityId.HasValue && otherCityId.Value > 0)
                DPSheetPageVM = DPSheetPageModel.GetPriceSheetInformation(cityId, makeId, dealerId, otherCityId.Value);
            else
                DPSheetPageVM = DPSheetPageModel.GetPriceSheetInformation(cityId, makeId, dealerId);

            return View(DPSheetPageVM);
        }
    }
}