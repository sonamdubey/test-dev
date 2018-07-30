using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking.Make
{
    /// <summary>
    /// Bikebooking Make list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBMakeList
    {
        [JsonProperty("makes")]
        public IEnumerable<BBMakeBase> Makes { get; set; }
    }
}
