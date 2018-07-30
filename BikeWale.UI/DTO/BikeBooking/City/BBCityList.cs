using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking.City
{
    /// <summary>
    /// Bikebooking City list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBCityList
    {
        [JsonProperty("cities")]
        public IEnumerable<BBCityBase> Cities { get; set; }
    }
}
