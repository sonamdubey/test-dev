
using Newtonsoft.Json;
namespace Bikewale.DTO.MobileVerification
{
    public class MobileSmsVerification
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("mobilenumber")]
        public string MobileNumber { get; set; }
        [JsonProperty("pageurl")]
        public string PageUrl { get; set; }
    }
}
