using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DiscountSummaryDTO_AndroidV1
    {
        [JsonProperty("maxDiscount")]
        public int MaxDiscount { get; set; }

        [JsonProperty("stockCount")]
        public int DealsCount { get; set; }

        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("linkText")]
        public string LinkText { get; set; }
    }
}
