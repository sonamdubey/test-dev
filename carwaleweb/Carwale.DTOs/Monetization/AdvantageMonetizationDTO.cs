using Carwale.DTOs.Deals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Monetization
{
    public class AdvantageMonetizationDTO : MonetizationDataPriorityDTO
    {
        [JsonProperty("advantageDiscountSummary")]
        public DiscountSummaryDTO_AndroidV1 AdvantageDiscountSummary { get; set; }
    }
}
