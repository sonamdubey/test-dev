using Newtonsoft.Json;

namespace Bikewale.DTO.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version DTO
    /// Created On  :   07 Dec 2015
    /// </summary>
    public class AppVersion
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("isSupported")]
        public bool IsSupported { get; set; }
        [JsonProperty("isLatest")]
        public bool IsLatest { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("code")]
        public AppVersionMessageCode Code { get; set; }
        [JsonProperty("isTrackDayVisible")]
        public bool IsTrackDayVisible { get; set; }  //to handle track day option in navigation menu for app
    }
}
