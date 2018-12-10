using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class SimilarCarModels
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int IsFeatured { get; set; }
        public string SpotlightUrl { get; set; }
        public int FeaturedModelId { get; set; }
        public string LargePic { get; set; }
        public string SmallPic { get; set; }
        public decimal ReviewRate { get; set; }
        public int ReviewCount { get; set; }
        public int PopularVersionId { get; set; }
        public string ModelImageUrl { get; set; }
        public string HostUrl { get; set; }
        public string ModelImageOriginal { get; set; }
        public string ModelImageLargeUrl { get; set; }
        [JsonProperty(PropertyName = "makeName")]
        public string MakeName { get; set; }

        [JsonProperty(PropertyName = "modelName")]
        public string ModelName { get; set; }

        [JsonProperty(PropertyName = "modelId")]
        public int ModelId { get; set; }

        [JsonProperty(PropertyName = "maskingName")]
        public string MaskingName { get; set; }


        public int MinAvgPrice { get; set; }

        public PriceOverview PricesOverview { get; set; }
        public string CompareCarUrl { get; set; }
        public DateTime LaunchDate { get; set; }
        public bool New { get; set; }
        public bool Futurstic { get; set; }
    }


}
