using Bikewale.DTO.AutoBiz;
using System.Collections.Generic;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Oct 2015
    /// Modified By : Sushil Kumar on 10th January 2016
    /// Description : Added provision to retrieve bike availability by color
    /// </summary>
    public class DealerPriceQuoteEntityDTO
    {
        public IEnumerable<PQ_PriceDTO> PriceList { get; set; }
        public IEnumerable<OfferEntityBaseDTO> OfferList { get; set; }
        public IEnumerable<DealerQuotationDTO> DealerDetails { get; set; }
        public IEnumerable<BikeColorAvailabilityDTO> BikeAvailabilityByColor { get; set; }
    }
}