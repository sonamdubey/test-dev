using Newtonsoft.Json;
namespace Carwale.DTOs.CarData
{
    public class CarMakesDTOV1
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }                         
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("used")]
        public bool Used { get; set; }
        [JsonProperty("new")]
        public bool New { get; set; }
        [JsonProperty("indian")]
        public bool Indian { get; set; }
        [JsonProperty("imported")]
        public bool Imported { get; set; }
        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }
        [JsonProperty("priortyOrder")]
        public int PriorityOrder { get; set; }
    }
}
