
using Newtonsoft.Json;
namespace Bikewale.DTO.MobileVerification
{
    /// <summary>
    /// Modified by : Snehal Dange on 18th Jan 2018
    /// Description :  Added AgentType 
    /// </summary>
    public class MobileSmsVerification
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("mobilenumber")]
        public string MobileNumber { get; set; }
        [JsonProperty("pageurl")]
        public string PageUrl { get; set; }
        [JsonProperty("agentType")]
        public sbyte AgentType { get; set; }
    }
}
