using Bikewale.DTO.PriceQuote.BikeBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BookingSummary
{
    /// <summary>
    /// Booking summary base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// Modified By : Sushil Kumar on 7th Dec 2015
    /// Description : Added varients with min specs and price
    /// </summary>
    public class BookingSummaryBase
    {
        [JsonProperty("dealerQuotation")]
        public Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase DealerQuotation { get; set; }

        [JsonProperty("customer")]
        public Bikewale.DTO.PriceQuote.CustomerDetails.PQCustomer Customer { get; set; }

        [JsonProperty("varients")]
        public IList<BikeDealerPriceDetailDTO> Varients { get; set; }
    }
}
