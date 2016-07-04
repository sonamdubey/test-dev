using System.Collections.Generic;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    public class PQ_DealerDetailEntity
    {
        public NewBikeDealers objDealer { get; set; }
        public PQ_QuotationEntity objQuotation { get; set; }
        public List<OfferEntity> objOffers { get; set; }
        public List<FacilityEntity> objFacilities { get; set; }
        public EMI objEmi { get; set; }
        public BookingAmountEntityBase objBookingAmt { get; set; }
    }
}
