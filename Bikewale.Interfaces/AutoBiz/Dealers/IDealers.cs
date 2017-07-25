using Bikewale.Entities.BikeBooking;
using System.Data;

namespace Bikewale.Interfaces.AutoBiz
{
    /// <summary>
    /// Modified by :   Sumit Kate on 10 Mar 2016
    /// Description :   Added new function GetDealerBenefits, DeleteDealerBenefits, SaveDealerBenefit
    /// Modified by :   Sumit Kate on 11 Mar 2016
    /// Description :   Added new function to Save the Dealer EMI values
    /// Modified by :   Sangram Nandkhile on 15 Mar 2016
    /// Description :   made changes in signature of SaveDealerEMI
    /// </summary>
    public interface IDealers
    {
        PQ_DealerDetailEntity GetDealerDetailsPQ(PQParameterEntity objParams);
        DataTable GetDealerCities();
        uint GetAvailabilityDays(uint dealerId, uint versionId);
        BookingAmountEntity GetDealerBookingAmount(uint versionId, uint dealerId);
    }
}
