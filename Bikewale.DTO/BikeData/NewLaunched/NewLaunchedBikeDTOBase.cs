using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Feb 2017
    /// Description :   New Launched Bike DTO Base
    /// </summary>
    public class NewLaunchedBikeDTOBase
    {
        [JsonProperty("make")]
        public Make.MakeBase Make { get; set; }
        [JsonProperty("model")]
        public Model.ModelBase Model { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("reviewCount")]
        public uint ReviewCount { get; set; }
        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }
        [JsonProperty("minPrice")]
        public uint MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public uint MaxPrice { get; set; }
        [JsonProperty("minSpec")]
        public IEnumerable<v2.VersionMinSpecs> MinSpecsList { get; set; }
        [JsonIgnore]
        public DateTime LaunchedOn { get; set; }
        [JsonProperty("launchedOn")]
        public String DisplayLaunchDate { get { return LaunchedOn.ToString("dd MMM yyyy"); } }
        [JsonProperty("launchedOnLong")]
        public String DisplayLaunchDateLong { get { return LaunchedOn.ToString("dd MMMM yyyy"); } }
        [JsonProperty("city")]
        public City.CityBase City { get; set; }
        [JsonProperty("price")]
        public uint Price { get; set; }
        [JsonProperty("bikeName")]
        public String BikeName { get { return string.Format("{0} {1}", (Make != null) ? Make.MakeName : "", (Model != null) ? Model.ModelName : ""); } }
    }
}
