using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OffersV1;
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
    public class ModelPageDTOApp_V2
    {
        [JsonProperty("modelDetails")]
        public CarModelDetailsDtoV2 ModelDetails { get; set; }

        [JsonProperty("modelColors")]
        public List<ModelColorsDTO> ModelColors { get; set; }

        [JsonProperty("modelVideos")]
        public List<VideoDTO> ModelVideos { get; set; }

        [JsonProperty("alternateCars")]
        public List<SimilarCarModelsDtoV3> SimilarCars { get; set; }

        [JsonProperty("modelVersions")]
        public List<CarVersionDtoV3> NewCarVersions { get; set; }

        [JsonProperty("callSlugNumber")]
        public string CallSlugNumber { get; set; }

        [JsonProperty("mileageData")]
        public List<MileageDataDTO_V1> MileageData { get; set; }

        [JsonProperty("emiInformation")]
        public EmiInformationDtoV2 EmiInfo { get; set; }

        [JsonProperty("orpText")]
        public string OrpText { get; set; }

        [JsonProperty("priceBreakUpText")]
        public string PriceBreakUpText { get; set; }

        [JsonProperty("city")]
        public CityDTO City { get; set; }

        [JsonProperty("offers")]

        public OfferDto Offer { get; set; }
    }
}
