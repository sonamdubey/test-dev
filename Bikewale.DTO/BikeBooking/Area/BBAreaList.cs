using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking.Area
{
    /// <summary>
    /// Bikebooking Area List
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBAreaList
    {
        [JsonProperty("areas")]
        public IEnumerable<BBAreaBase> Areas { get; set; }
    }
}
