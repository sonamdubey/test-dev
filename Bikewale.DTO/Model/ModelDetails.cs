using Bikewale.DTO.Make;
using Bikewale.DTO.Series;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Model
{
    public class ModelDetails : ModelBase
    {
        [JsonProperty("makeDetails")]
        private MakeBase objmakeBase = new MakeBase();
        public MakeBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        [JsonProperty("seriesDetails")]
        private SeriesBase objEntityBase = new SeriesBase();
        public SeriesBase ModelSeries { get { return objEntityBase; } set { objEntityBase = value; } }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }


        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("smallPic")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePic")]
        public string LargePicUrl { get; set; }

        [JsonProperty("minPrice")]
        public Int64 MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public Int64 MaxPrice { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

    }
}
