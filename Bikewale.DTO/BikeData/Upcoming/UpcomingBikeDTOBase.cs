using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData.Upcoming
{
    public class UpcomingBikeDTOBase
    {
        [JsonProperty("expectedLaunchedDate")]
        public DateTime ExpectedLaunchedDate { get; set; }
        [JsonProperty("estimatedPriceMin")]
        public ulong EstimatedPriceMin { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("makeBase")]
        public Make.MakeBase MakeBase { get; set; }
        [JsonProperty("modelBase")]
        public Model.ModelBase ModelBase { get; set; }
        [JsonProperty("isLaunchingThisMonth")]
        public bool IsLaunchingThisMonth { get; set; }
    }
}
