using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>

    public class PQ_DealerDetailEntity
    {
        public NewBikeDealers objDealer { get; set; }
        public Bikewale.Entities.BikeBooking.PQ_QuotationEntity objQuotation { get; set; }
        public List<OfferEntity> objOffers { get; set; }
        public List<FacilityEntity> objFacilities { get; set; }
        public EMI objEmi { get; set; }
        public BookingAmountEntityBase objBookingAmt { get; set; }
    }
}
