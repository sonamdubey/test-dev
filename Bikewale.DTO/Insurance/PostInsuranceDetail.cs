using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Insurance
{
    public class PostInsuranceDetail
    {   
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Mobile")]
        public string Mobile { get; set; }

        [JsonProperty("CityName")]
        public string CityName { get; set; }

        [JsonProperty("Price")]
        public int Price { get; set; }

        [JsonProperty("CarPurchaseDate")]
        public DateTime CarPurchaseDate { get; set; }

        [JsonProperty("CityId")]
        public int CityId { get; set; }

        [JsonProperty("MakeId")]
        public int MakeId { get; set; }

        [JsonProperty("ModelId")]
        public int ModelId { get; set; }

        [JsonProperty("VersionId")]
        public int VersionId { get; set; }

        [JsonProperty("InsuranceNew")]
        public bool InsuranceNew { get; set; }
    }
}
