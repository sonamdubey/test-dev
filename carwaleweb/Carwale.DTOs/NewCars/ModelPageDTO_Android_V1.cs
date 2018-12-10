using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Jitendra 
    /// </summary>
    /// 
    public class ModelPageDTO_Android_V1
    {
        [JsonProperty("modelDetails")]
        public CarModelDetailsDTO_V1 ModelDetails { get; set; }

        [JsonProperty("modelColors")]
        public List<ModelColorsDTO> ModelColors { get; set; }

        [JsonProperty("modelVideoList")]
        public List<VideoDTO> ModelVideos { get; set; }

        [JsonProperty("alternativeCars")]
        public List<SimilarCarModelsDTO_V1> SimilarCars { get; set; }

        [JsonProperty("modelVersions")]
        public List<CarVersionDTO_V1> NewCarVersions { get; set; }

        [JsonProperty("callSlugNumber")]
        public string CallSlugNumber { get; set; }


        [JsonProperty("discontinuedCarVersion")]
        public List<CarVersionDTO_V1> DiscontinuedCarVersion { get; set; }

        [JsonProperty("mileageData")]
        public List<MileageDataDTO_V1> MileageData { get; set; }

        [JsonProperty("emiInformation")]
        public EMIInformationDTO EmiInfo { get; set; }
    }
}
