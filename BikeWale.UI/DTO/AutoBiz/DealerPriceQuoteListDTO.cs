using System.Collections.Generic;

namespace BikeWale.DTO.AutoBiz
{
    public class DealerPriceQuoteListDTO
    {
        public IEnumerable<DealerPriceQuoteDetailedDTO> DealersDetails { get; set; }
    }
}