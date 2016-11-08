using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 07 Nov 2016
    /// Description : Service center data on city listing page.
    /// </summary>
    public class ServiceCenterData
    {
        [JsonProperty("count")]
        public uint Count { get; set; }

        [JsonProperty("serviceCenters")]
        public IEnumerable<ServiceCenterDetails> ServiceCenters { get; set; }
    }
}
