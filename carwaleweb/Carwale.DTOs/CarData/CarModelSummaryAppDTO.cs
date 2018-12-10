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
    public class CarModelSummaryAppDTO
    {

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonIgnore]
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonIgnore]
        [JsonProperty("modelRating")]
        public double ModelRating { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonIgnore]
        [JsonProperty("minPrice")]
        public double MinPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("maxPrice")]
        public double MaxPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("modelImage")]
        public string ModelImage { get; set; }

        [JsonIgnore]
        [JsonProperty("largeImage")]
        public string LargeImage { get; set; }

        [JsonIgnore]
        [JsonProperty("xLargeImage")]
        public string XLargeImage { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImage { get; set; }

        [JsonIgnore]
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonIgnore]
        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("offerExists")]
        public bool OfferExists { get; set; }

        [JsonIgnore]
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonIgnore]
        [JsonProperty("discountSummary")]
        public DiscountSummaryDTO DiscountSummaryDTO { get; set; }

        [JsonProperty("reviewRate")]
        public string ModelRatingNew { get; set; }
    }
}

