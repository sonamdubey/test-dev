using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking.Version
{
    /// <summary>
    /// Bikebooking verison list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBVersionList
    {
        [JsonProperty("versions")]
        public IEnumerable<BBVersionBase> Versions { get; set; }
    }
}
