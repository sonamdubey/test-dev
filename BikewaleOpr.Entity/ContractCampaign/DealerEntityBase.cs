using Newtonsoft.Json;

namespace BikewaleOpr.Entity.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 18 Jan 2017
    /// Description :   Dealer Entity Base
    /// </summary>
    public class DealerEntityBase
    {
        [JsonProperty("Id")]
        public uint Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
