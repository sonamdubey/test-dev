using Newtonsoft.Json;
namespace Bikewale.DTO.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 07/10/2016
    /// Description : To store inquiry details retieved by profile Id.
    /// </summary>
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
        public string Url { get { return string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/", CityMaskingName, MakeMaskingName, ModelMaskingName, ProfileId); } }
    }
}
