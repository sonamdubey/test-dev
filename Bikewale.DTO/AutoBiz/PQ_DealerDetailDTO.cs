using Bikewale.DTO.AutoBiz;
using System.Collections.Generic;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>

    public class PQ_DealerDetailEntityDTO
    {
        public NewBikeDealersDTO objDealer { get; set; }
        public PQ_QuotationEntityDTO objQuotation { get; set; }
        public List<OfferEntityDTO> objOffers { get; set; }
        public List<FacilityEntityDTO> objFacilities { get; set; }
        public EMIDTO objEmi { get; set; }
        public BookingAmountEntityBaseDTO objBookingAmt { get; set; }
    }
}
