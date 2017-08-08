using Newtonsoft.Json;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Written By : Ashwini Todkar on 29 Oct 2014
    /// Modified By:Snehal Dange on 7th August 2017
    /// </summary>
    public class FacilityEntity
    {
        [JsonProperty("facility")]
        public string Facility { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        public uint FacilityId { get; set; }
    }
}
