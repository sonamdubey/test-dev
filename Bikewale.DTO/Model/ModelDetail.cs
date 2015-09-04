using Bikewale.DTO.Make;
using Bikewale.DTO.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class ModelDetail  : ModelBase
    {
        [JsonProperty("makeBase")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty("seriesBase")]
        public SeriesBase ModelSeries { get; set; }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("minPrice")]
        public Int64 MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public Int64 MaxPrice { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
    }
}
