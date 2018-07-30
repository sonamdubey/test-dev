using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking.Model
{
    /// <summary>
    /// Bikebooking model list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBModelList
    {
        [JsonProperty("models")]
        public IEnumerable<BBModelBase> Models { get; set; }
    }
}
