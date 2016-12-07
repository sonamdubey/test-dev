using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 29 Oct 2014
    /// Modified By : Lucky Rathore on 06 June 2016 
    /// Description : DealerInfo IsDealerExists(uint versionId, uint areaId) Added.
    /// Modified By : Sushil Kumar on 29th Nov 2016
    /// Description : Added function feature 
    ///                 1.UpdateDealerDailyLeadCount
    ///                 2.IsDealerDailyLeadLimitExceeds
    ///               to pass autobiz leads only when dealer leads does not exceeds daily limit count    
    /// </summary>
    public interface IDealerPriceQuote
    {
        bool SaveCustomerDetail(DPQ_SaveEntity entity); // Modified By : Sumit Kate on 29 Dec 2015
        bool UpdateIsMobileVerified(uint pqId);
        bool UpdateMobileNumber(uint pqId, string mobileNo);
        bool PushedToAB(uint pqId, uint abInquiryId);
        PQCustomerDetail GetCustomerDetails(uint pqId);
        bool IsNewBikePQExists(uint pqId);
        List<BikeVersionEntityBase> GetVersionList(uint versionId, uint dealerId, uint cityId);
        bool SaveRSAOfferClaim(RSAOfferClaimEntity objOffer, string bikeName);
        bool UpdatePQBikeColor(uint colorId, uint pqId);
        //VersionColor GetPQBikeColor(uint pqId);
        bool UpdatePQTransactionalDetail(uint pqId, uint transId, bool isTransComplete, string bookingReferenceNo);
        bool IsDealerNotified(uint dealerId, string customerMobile, ulong customerId);
        bool IsDealerPriceAvailable(uint versionId, uint cityId);
        uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId);
        List<Bikewale.Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId);
        PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams);
        BookingPageDetailsEntity FetchBookingPageDetails(uint cityId, uint versionId, uint dealerId);
        DealerInfo IsDealerExists(uint versionId, uint areaId);
        bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId);
        bool IsDealerDailyLeadLimitExceeds(uint campaignId);
    }
}