using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Modified by :  Sangram Nandkhile on 06 Jul 2016
    /// </summary>
    public interface IDealerPriceQuote
    {
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        List<PQ_Price> GetBikeCategoryItems(string catgoryList);
        bool SaveDealerPrice(uint dealerId, uint versionId, uint cityId, UInt16 itemId, UInt32 itemValue);
        bool SaveDealerPrice(DataTable dt);
        DataSet GetDealerPrices(uint cityId, uint makelId, uint dealerId);
        bool MapDealerWithArea(uint dealerId, string areaIdList);
        bool UnmapDealer(uint dealerId, string areaIdList);
        List<DealerAreaDetails> GetDealerAreaDetails(uint cityId);
        //DealerInfo GetCampaignDealersLatLongV3(uint versionId, uint areaId);
        void GetAreaLatLong(uint areaId, out double lattitude, out double longitude);
        List<DealerLatLong> GetDealersLatLong(uint versionId, uint areaId);
        DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds);
    }
}
