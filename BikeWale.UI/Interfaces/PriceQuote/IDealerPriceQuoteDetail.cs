using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using System;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 March 2016
    /// Description : for Dealer Price quote page functionalies.
    /// Modified By : Sumit Kate on 3rd June 2016
    /// Description : New Quotation method for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public interface IDealerPriceQuoteDetail
    {
        DetailedDealerQuotationEntity GetDealerQuotation(UInt32 cityId, UInt32 versionID, UInt32 dealerId);
        Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerQuotationV2(UInt32 cityId, UInt32 versionID, UInt32 dealerId, uint areaId);
        PQ_QuotationEntity Quotation(uint cityId, UInt16 sourceType, string deviceId, uint dealerId, uint modelId, ref ulong pqId, bool isPQRegistered, uint? areaId = null, uint? versionId = null);
        PQ_QuotationEntity QuotationV2(uint cityId, UInt16 sourceType, string deviceId, uint dealerId, uint modelId, ref string pqId, bool isPQRegistered, uint? areaId = null, uint? versionId = null);
    }
}
