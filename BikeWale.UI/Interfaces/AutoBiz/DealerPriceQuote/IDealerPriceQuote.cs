using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using BikeWale.Entities.AutoBiz;
using System;
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
        List<CityEntityBase> GetBikeBookingCities(uint? modelId);
        List<BikeMakeEntityBase> GetBikeMakesInCity(uint cityId);
        OfferHtmlEntity GetOfferTerms(string offerMaskingName, int? offerId);
        DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds);
        DetailedDealerQuotationEntity GetDealerPriceQuoteByPackage(PQParameterEntity objParams);
        Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerPriceQuoteByPackageV2(PQParameterEntity objParams);
        DealerInfo GetNearestDealer(uint modelId, uint cityId);
        DealerInfo GetNearestDealer(uint modelId, uint cityId, uint areaId);
        IEnumerable<PQ_VersionPrice> GetDealerPriceQuotesByModelCity(uint cityId, uint modelId, uint dealerId);
    }
}