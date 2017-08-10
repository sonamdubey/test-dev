using Newtonsoft.Json;
using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Written By : Ashwini Todkar on 29 Oct 2014
    /// Modified By:Snehal Dange on 7th August 2017
    /// Description : Added coloumns 'FacilityId','LastUpdatedOn' and 'LastUpdatedBy'
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

        public DateTime LastUpdatedOn { get; set; }

        public UInt16 LastUpdatedById { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}
