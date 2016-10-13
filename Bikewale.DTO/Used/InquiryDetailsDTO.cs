using Newtonsoft.Json;
namespace Bikewale.DTO.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 07/10/2016
    /// Description : To store inquiry details retieved by profile Id.
    /// </summary>
    public class InquiryDetailsDTO
    {
        [JsonIgnore]
        public uint StatusId { get; set; }
        [JsonIgnore]
        public string CityMaskingName { get; set; }
        [JsonIgnore]
        public string MakeMaskingName { get; set; }
        [JsonIgnore]
        public string ModelMaskingName { get; set; }
        [JsonIgnore]
        public string ProfileId { get; set; }
        [JsonProperty("url")]
        public string Url { get { return string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/", CityMaskingName, MakeMaskingName, ModelMaskingName, ProfileId); } }
        [JsonProperty("isRedirect")]
        public bool IsRedirect { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
