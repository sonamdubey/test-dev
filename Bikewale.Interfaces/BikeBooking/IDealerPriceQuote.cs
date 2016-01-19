using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Customer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Interfaces.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 29 Oct 2014
    /// </summary>
    public interface IDealerPriceQuote
    {
        bool SaveCustomerDetail(DPQ_SaveEntity entity); // Modified By : Sumit Kate on 29 Dec 2015
        bool UpdateIsMobileVerified(uint pqId);
        bool UpdateMobileNumber(uint pqId, string mobileNo);
        bool PushedToAB(uint pqId, uint abInquiryId);
#if unused
        bool UpdateAppointmentDate(uint pqId, DateTime date);
#endif
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
    }
}