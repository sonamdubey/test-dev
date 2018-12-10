using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.PopularUC
{
    public class PopularUCModelDesktop
    {
        [JsonProperty("carName")]
        public string carName { get; set; }
        [JsonProperty("price")]
        public string price { get; set; }
        [JsonProperty("Image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("titleUrl")]
        public string titleUrl { get; set; }
        [JsonProperty("viewSimilarUrl")]
        public string viewSimilarUrl { get; set; }
        [JsonProperty("bodyType")]
        public string bodyType { get; set; }
    }
}
