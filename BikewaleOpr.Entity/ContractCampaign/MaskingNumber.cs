using Newtonsoft.Json;


namespace BikewaleOpr.Entity.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sushil Kumar on 14 July 2016
    /// Description :   Opr Entity for Masking Number
    /// </summary>
    public class MaskingNumber
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("isAssigned")]
        public bool IsAssigned { get; set; }
    }
}
