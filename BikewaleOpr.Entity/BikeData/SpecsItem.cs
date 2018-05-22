using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created By  : Ashutosh Sharma on 01 Apr 2018
    /// Description : Entity for Specs Item.
    /// </summary>
    public class SpecsItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
