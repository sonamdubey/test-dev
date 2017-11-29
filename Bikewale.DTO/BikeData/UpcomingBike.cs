using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData
{

    public class UpcomingBike
    {
        [JsonProperty(PropertyName = "id")]
        public uint ExpectedLaunchId { get; set; }

        [JsonProperty(PropertyName = "launchDate")]
        public string ExpectedLaunchDate { get { return ExpectedLaunchedDate.ToString("MMM yyyy"); } }

        [JsonProperty(PropertyName = "minPrice")]
        public ulong EstimatedPriceMin { get; set; }

        [JsonProperty(PropertyName = "maxPrice")]
        public ulong EstimatedPriceMax { get; set; }

        [JsonProperty(PropertyName = "hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty(PropertyName = "imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty(PropertyName = "makeBase")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty(PropertyName = "modelBase")]
        public ModelBase ModelBase { get; set; }

        [JsonProperty(PropertyName = "bikeDesc")]
        public BikeDiscription BikeDescription { get; set; }

        [JsonProperty(PropertyName = "isLaunchingThisMonth")]
        public bool IsLaunchingThisMonth { get; set; }

        [JsonIgnore]
        public DateTime ExpectedLaunchedDate {get; set;}
    }
}
