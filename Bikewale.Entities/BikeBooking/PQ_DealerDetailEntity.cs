using Bikewale.Entity.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>
    
    [Serializable]
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
