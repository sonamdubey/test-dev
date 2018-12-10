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
    public class SimilarCarModelsDTO
    {
        [JsonIgnore]
        [JsonProperty("minPrice")]
        public double MinPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("maxPrice")]
        public double MaxPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("isFeatured")]
        public int IsFeatured { get; set; }

        [JsonIgnore]
        [JsonProperty("spotlightUrl")]
        public string SpotlightUrl { get; set; }

        [JsonIgnore]
        [JsonProperty("featuredModelId")]
        public int FeaturedModelId { get; set; }

        [JsonIgnore]
        [JsonProperty("largePicUrl")]
        public string LargePic { get; set; }

        [JsonIgnore]
        [JsonProperty("smallPicUrl")]
        public string SmallPic { get; set; }

        [JsonIgnore]
        [JsonProperty("reviewRateOld")]
        public decimal ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("versionId")]
        public int PopularVersionId { get; set; }

        [JsonIgnore]
        [JsonProperty("modelImageUrl")]
        public string ModelImageUrl { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string ModelImageOriginal { get; set; }

        [JsonIgnore]
        [JsonProperty("modelImageLargeUrl")]
        public string ModelImageLargeUrl { get; set; }

        [JsonProperty("make")]
        public string MakeName { get; set; }

        [JsonProperty("model")]
        public string ModelName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonIgnore]
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }


        [JsonProperty("reviewRate")]
        public string ReviewRateNew;

        
        [JsonProperty("minPrice")]
        public string MinPriceNew { get; set; }

        [JsonIgnore]
        [JsonProperty("maxPrice")]
        public string MaxPriceNew { get; set; }

        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }

        [JsonProperty("exShowroomCityId")]
        public int ExShowRoomCityId { get; set; }

        [JsonProperty("exShowroomCity")]
        public string ExShowRoomCityName { get; set; }
        
    }
}
