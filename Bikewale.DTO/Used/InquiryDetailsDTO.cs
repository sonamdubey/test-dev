using Newtonsoft.Json;
namespace Bikewale.DTO.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 07/10/2016
    /// Description : To store inquiry details retieved by profile Id.
    /// </summary>
    public class InquiryDetailsDTO
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("isRedirect")]
        public bool IsRedirect { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
