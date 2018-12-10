using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
    public class CarDetailsDTO
    {
        [JsonProperty("carMake")]
        public CarMakesDTO CarMake;

        [JsonProperty("carModel")]
        public CarModelsDTO CarModel;

        [JsonProperty("carVersion")]
        public PQCarVersionDTO CarVersion;

        [JsonProperty("carImageBase")]
        public CarImageBaseDTO CarImageBase;

        [JsonProperty("carName")]
        public string CarName;

        [JsonProperty("versionDetailUrl")]
        public string VersionDetailUrl { get; set; }

        [JsonProperty("modelDetailUrl")]
        public string ModelDetailUrl { get; set; }

        [JsonProperty("bodyType")]
        public BodyTypeBaseDTO BodyType { get; set; }
    }
}
