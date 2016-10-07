using Newtonsoft.Json;
namespace Bikewale.DTO.Used
{
    public class InquiryDetailsDTO
    {
        [JsonProperty("statusId")]
        public uint StatusId { get; set; }
        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
