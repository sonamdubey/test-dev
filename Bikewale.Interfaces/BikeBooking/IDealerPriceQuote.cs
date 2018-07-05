using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
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
    /// Modified by :   Sumit Kate on 16 Dec 2016
    /// Description :   Added GetDefaultPriceQuoteVersion(uint modelId, uint cityId, uint areaId)
    /// Modified by : Sangram Nandkhile on 14 Feb
    /// Summary : Added function ProcessPQV2
    /// Modified by : Kartik Rathod on 20 jun 2018 added ProcessPQV3 price qoute changes
    /// </summary>
    public interface IDealerPriceQuote
    {
        uint SaveCustomerDetailByPQId(DPQ_SaveEntity entity); // Modified By : Sumit Kate on 29 Dec 2015
        uint SaveCustomerDetailByLeadId(Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity);
        bool UpdateIsMobileVerified(uint pqId);
        bool UpdateMobileNumber(uint pqId, string mobileNo);
        bool PushedToAB(uint pqId, uint abInquiryId);
        PQCustomerDetail GetCustomerDetailsByPQId(uint pqId);
        bool IsNewBikePQExists(uint pqId);
        List<BikeVersionEntityBase> GetVersionList(uint versionId, uint dealerId, uint cityId);
        bool SaveRSAOfferClaim(RSAOfferClaimEntity objOffer, string bikeName);
        bool UpdatePQBikeColor(uint colorId, uint pqId);
        //VersionColor GetPQBikeColor(uint pqId);
        bool UpdatePQTransactionalDetail(uint pqId, uint transId, bool isTransComplete, string bookingReferenceNo);
        bool IsDealerNotified(uint dealerId, string customerMobile, ulong customerId);
        bool IsDealerPriceAvailable(uint versionId, uint cityId);
        uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId);
        uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId, uint areaId);
        List<Bikewale.Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId);
        PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams, bool isManufacturerCampaignRequired = false);
        Bikewale.Entities.BikeBooking.v2.PQOutputEntity ProcessPQV2(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams, bool isDealerSubscriptionRequired = true);
        BookingPageDetailsEntity FetchBookingPageDetails(uint cityId, uint versionId, uint dealerId);
        bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId);
        bool IsDealerDailyLeadLimitExceeds(uint campaignId);
        Bikewale.Entities.BikeBooking.v2.PQOutputEntity ProcessPQV3(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams);
        PQCustomerDetail GetCustomerDetailsByLeadId(uint leadId);
    }
}