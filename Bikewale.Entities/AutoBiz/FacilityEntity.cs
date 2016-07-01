using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 29 Oct 2014
    /// </summary>
    public class FacilityEntity
    {
        [JsonProperty("facility")]
        public string Facility { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
