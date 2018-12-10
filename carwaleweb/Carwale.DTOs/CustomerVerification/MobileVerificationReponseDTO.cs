using Newtonsoft.Json;

namespace Carwale.DTOs.CustomerVerification
{
    public class MobileVerificationReponseDto
    {
        [JsonProperty("isMobileVerified")]
        public bool IsMobileVerified { get; set; }
        public bool IsOtpGenerated { get; set; }
        [JsonProperty("tollFreeNumber")]
        public string TollFreeNumber { get; set; }
    }
}
