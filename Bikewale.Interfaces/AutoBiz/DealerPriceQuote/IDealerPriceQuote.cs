using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.Interfaces.AutoBiz
{
    /// <summary>
    /// Modified by :   Sumit Kate on 15 Mar 2016
    /// Description :   Added GetDealerPriceQuoteByPackage to get the dealer price quote based on subscription model.
    /// Modified by :   Sumit Kate on 21 Mar 2016
    /// Description :   Added GetCampaignDealersLatLong to get the Dealers Lattitude and Longitude based on subscription model
    /// </summary>
    public interface IDealerPriceQuote
    {
        PQ_QuotationEntity GetDealerPriceQuote(PQParameterEntity objParams);
        List<PQ_Price> GetBikeCategoryItems(string catgoryList);
        uint IsDealerExists(uint versionId, uint areaId);
        List<DealerAreaDetails> GetDealerAreaDetails(uint cityId);
        void GetAreaLatLong(uint areaId, out double lattitude, out double longitude);
        List<CityEntityBase> GetBikeBookingCities(uint? modelId);
        List<BikeMakeEntityBase> GetBikeMakesInCity(uint cityId);
        OfferHtmlEntity GetOfferTerms(string offerMaskingName, int? offerId);
        DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds);
        DetailedDealerQuotationEntity GetDealerPriceQuoteByPackage(PQParameterEntity objParams);
        DealerInfo GetCampaignDealersLatLongV3(uint versionId, uint areaId);
        Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerPriceQuoteByPackageV2(PQParameterEntity objParams);
    }
}