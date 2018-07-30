using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.DealerLocator.v2
{
    /// <summary>
    /// Created by  :   Sumit Kate on 20 May 2016
    /// Description :   bikes list wrapper class
    /// </summary>
    public class DealerBikes
    {
        [JsonProperty("Version")]
        public IEnumerable<DealerBikeBase> Bikes { get; set; }
    }
}
