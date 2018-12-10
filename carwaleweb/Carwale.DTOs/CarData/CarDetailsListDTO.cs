using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    public class CarDetailsListDTO
    {
        [JsonProperty("models")]
        public List<VersionDetailsDTO> Models;
    }
    public class VersionDetailsBaseDTO
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("versionDetails")]
        public List<VersionListDTO> VersionDetails { get; set; }
        [JsonProperty("threeSixtyAvailability")]
        public ThreeSixtyAvailabilityDTO ThreeSixtyAvailability { get; set; }
    }
    public class VersionDetailsDTO : VersionDetailsBaseDTO { }
   
    public class VersionDetailsDtoV2 : VersionDetailsBaseDTO
    {
        [JsonProperty("priceBreakUpText")]
        public string PriceBreakUpText { get; set; }
    }
    public class VersionListDTO 
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("reviewRate")]
        public float ReviewRate { get; set; }
        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }
        [JsonProperty("emiInformation")]
        public EMIInformationDTO EmiInfo { get; set; }
        [JsonProperty("fuelType")]
        public string FuelType { get; set; }
        [JsonProperty("transmissionType")]
        public string TransmissionType { get; set; }
        [JsonProperty("offers")]
        public OfferDto Offer { get; set; }
    }
}
