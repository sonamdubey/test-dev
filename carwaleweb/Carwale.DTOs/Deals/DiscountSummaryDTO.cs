using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carwale.DTOs.Deals
{
   public class DiscountSummaryDTO
    {
        [JsonProperty("maxDiscount")]
        public int MaxDiscount { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("dealsCount")]
        public int DealsCount { get; set; }

        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("offers")]
        public string Offers { get; set; }
    }
}
