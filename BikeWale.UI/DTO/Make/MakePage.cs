using Bikewale.DTO.Widgets;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Make
{
    /// <summary>
    /// Make Page DTO
    /// Author  :   Sumit Kate
    /// Date    :   03 Sept 2015
    /// </summary>
    public class MakePage
    {
        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public BikeDescription Description { get; set; }

        /// <summary>
        /// Popular Bikes
        /// </summary>
        [JsonProperty("popularBikes")]
        public IEnumerable<MostPopularBikes> PopularBikes { get; set; }
    }
}
