using Bikewale.DTO.Campaign;
using Bikewale.DTO.Model.v3;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v5
{
    /// <summary>
    /// Modified By : Rajan Chauhan on 22 Jan 2017
    /// Description : changed ModelColors from NewModelColorWithPhoto to ModelColorPhoto
    /// </summary>
    public class ModelPage
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("isDiscontinued")]
        public bool IsDiscontinued { get; set; }

        [JsonProperty("isUpcoming")]
        public bool IsUpcoming { get; set; }

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("isCityExists")]
        public bool IsCityExists { get; set; }

        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }

        [JsonProperty("smallDescription")]
        public string SmallDescription { get; set; }

        [JsonProperty("photos", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<CMSModelImageBase> Photos { get; set; }

        [JsonProperty("versionList", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<VersionDetail> ModelVersions { get; set; }

        [JsonProperty("modelColors", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ModelColorPhoto> ModelColors { get; set; }

        [JsonProperty("capacity")]
        public string Capacity { get; set; }

        [JsonProperty("mileage")]
        public string Mileage { get; set; }

        [JsonProperty("maxPower")]
        public string MaxPower { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("expectedLaunchDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExpectedLaunchDate { get; set; }

        [JsonProperty("expectedMinPrice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ulong ExpectedMinPrice { get; set; }

        [JsonProperty("expectedMaxPrice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ulong ExpectedMaxPrice { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("pqId")]
        public ulong PQId { get; set; }
        [JsonProperty("campaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CampaignBaseDto Campaign { get; set; }

        [JsonProperty("isSpecsAvailable")]
        public bool IsSpecsAvailable { get; set; }
        [JsonProperty("gallery")]
        public Gallery Gallery { get; set; }
        [JsonProperty("review")]
        public Review Review { get; set; }






    }
}
