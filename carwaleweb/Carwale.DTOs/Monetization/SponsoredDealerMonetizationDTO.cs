using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Monetization
{
    public class SponsoredDealerMonetizationDTO : MonetizationDataPriorityDTO
    {
        [JsonProperty("pqDealerAd")]
        public SponsoredDealerDTO PQDealerAd { get; set; }
    }
}
