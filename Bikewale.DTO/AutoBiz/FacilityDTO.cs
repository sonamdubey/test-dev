using Newtonsoft.Json;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 29 Oct 2014
    /// </summary>
    public class FacilityEntityDTO
    {
        [JsonProperty("facility")]
        public string Facility { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
