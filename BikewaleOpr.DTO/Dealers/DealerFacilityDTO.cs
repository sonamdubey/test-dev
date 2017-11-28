using Newtonsoft.Json;
using System;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Written By : Snehal Dange on 7th August 2017
    /// Description : Dto for dealer facility
    /// </summary>

    public class DealerFacilityDTO
    {
        [JsonProperty("facilityName")]
        public string Facility { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("dealerId")]
        public uint Id { get; set; }

        [JsonProperty("facilityId")]
        public uint FacilityId { get; set; }

        [JsonProperty("lastUpdatedById")]
        public UInt16 LastUpdatedById { get; set; }
    }
}
