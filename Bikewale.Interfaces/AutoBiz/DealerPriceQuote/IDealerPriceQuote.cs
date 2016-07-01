using Bikewale.Entities.AutoBiz;
using BikeWale.Entities.AutoBiz;
using System;
using System.Collections.Generic;
using System.Data;

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
        DataSet GetDealerPrices(uint cityId, uint makelId, uint dealerId);
        bool SaveDealerPrice(uint dealerId, uint versionId, uint cityId, UInt16 itemId, UInt32 itemValue);
        bool SaveDealerPrice(DataTable dt);
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        uint IsDealerExists(uint versionId, uint areaId);
        List<DealerAreaDetails> GetDealerAreaDetails(uint cityId);
        bool MapDealerWithArea(uint dealerId, string areaIdList);
        bool UnmapDealer(uint dealerId, string areaIdList);
        List<DealerLatLong> GetDealersLatLong(uint versionId, uint areaId);
        DealerLatLong GetCampaignDealersLatLong(uint versionId, uint areaId);
        void GetAreaLatLong(uint areaId, out double lattitude, out double longitude);
        List<CityEntityBase> GetBikeBookingCities(uint? modelId);
        List<MakeEntityBase> GetBikeMakesInCity(uint cityId);
        OfferHtmlEntity GetOfferTerms(string offerMaskingName, int? offerId);
        DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds);
        DetailedDealerQuotationEntity GetDealerPriceQuoteByPackage(PQParameterEntity objParams);
        DealerInfo GetCampaignDealersLatLongV3(uint versionId, uint areaId);
    }
}
