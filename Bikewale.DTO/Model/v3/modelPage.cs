using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v3
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 12 Apr Jan 2016
    /// Description :   This new DTO for Model Page API v3
    /// Modified By :   Lucky Rathore on 05 May 2016.
    /// Descritption :  IsUpcoming, ExpectedLaunchDate and ExpectedPrice added.
    /// Modified By:    Sangram Nandkhile on 05 May 2016
    /// Description:    Added ExpectedMinPrice, ExpectedMaxPrice
    /// Modified By :   Rajan Chauhan on 10 Apr 2018
    /// Description :   Changed ModelVersions from List to IEnumerable
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

        [JsonProperty("photos")]
        public List<CMSModelImageBase> Photos { get; set; }

        [JsonProperty("versionList")]
        public IEnumerable<VersionDetail> ModelVersions { get; set; }

        [JsonProperty("capacity")]
        public string Capacity { get; set; }

        [JsonProperty("mileage")]
        public string Mileage { get; set; }

        [JsonProperty("maxPower")]
        public string MaxPower { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("pqId")]
        public ulong PQId { get; set; }

        [JsonProperty("expectedLaunchDate")]
        public string ExpectedLaunchDate { get; set; }

        [JsonProperty("expectedMinPrice")]
        public ulong ExpectedMinPrice { get; set; }

        [JsonProperty("expectedMaxPrice")]
        public ulong ExpectedMaxPrice { get; set; }

    }
}
