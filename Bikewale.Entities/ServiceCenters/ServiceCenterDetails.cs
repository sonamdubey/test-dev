using Newtonsoft.Json;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 07 Nov 2016
    /// Description : Service center data on city listing page.
    /// </summary>
    public class ServiceCenterDetails
    {
        [JsonProperty("serviceCenterId")]
        public uint ServiceCenterId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }
}
     