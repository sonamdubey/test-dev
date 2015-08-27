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
    /// </summary>
    public class BookingSummaryBase
    {
        [JsonProperty("dealerQuotation")]
        public Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase DealerQuotation { get; set; }
        [JsonProperty("customer")]
        public Bikewale.DTO.PriceQuote.CustomerDetails.PQCustomer Customer { get; set; }
    }
}
