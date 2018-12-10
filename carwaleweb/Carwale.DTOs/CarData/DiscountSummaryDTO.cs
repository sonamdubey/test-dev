using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    /// <summary>
    /// Created By : Ajay Singh on 1 march 2016
    /// </summary>
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

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("modelName")] 
        public string ModelName { get; set; }

        [JsonProperty("offers")]
        public string Offers { get; set; }
    }
}
