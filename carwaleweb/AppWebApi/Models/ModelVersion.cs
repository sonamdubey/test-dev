using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /* Author: Rakesh Yadav
     * Date Created: 19 June 2013
     * Discription: Create Model CarVersion  */
    public class ModelVersion
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("features")]
        public string Features { get; set; }

        [JsonProperty("exShowroomPrice")]
        public string ExShowroomPrice { get; set; }

        [JsonProperty("versionUrl")]
        public string VersionUrl { get; set; }

        [JsonProperty("isMatchingCriteria")]
        public bool IsMatchingCriteria { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("fuelType")]
        public string FuelType { get; set; }

        [JsonProperty("exShowroomCityId")]
        public int ExShowRoomCityId;

        [JsonProperty("exShowroomCity")]
        public string ExShowRoomCityName;

    }
}