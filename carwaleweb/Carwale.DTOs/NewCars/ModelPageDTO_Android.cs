using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Carwale.DTOs.CarData;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini 
    /// </summary>
    public class ModelPageDTO_Android
    {
        [JsonProperty("modelDetails")]
        public CarModelDetailsDTO ModelDetails { get; set; }

        [JsonProperty("modelColors")]
        public List<ModelColorsDTO> ModelColors { get; set; }

        [JsonProperty("modelVideoList")]
        public List<VideoDTO> ModelVideos { get; set; }

        [JsonProperty("alternativeCars")]
        public List<SimilarCarModelsDTO> SimilarCars { get; set; }

        [JsonProperty("modelVersions")]
        public List<CarVersionDTO> NewCarVersions { get; set; }

        [JsonProperty("callSlugNumber")]
        public string CallSlugNumber { get; set; }

       
        [JsonProperty("discontinuedCarVersion")]
        public List<CarVersionDTO> DiscontinuedCarVersion { get; set; }
       
    }
}
