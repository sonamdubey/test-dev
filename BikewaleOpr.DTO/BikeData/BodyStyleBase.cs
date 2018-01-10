using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created By: Rajan Chauhan on 12th Dec 2017
    /// </summary>
    public class BodyStyleBase
    {
        [JsonProperty("bodyStyleId")]
        public uint BodyStyleId { get; set; }

        [JsonProperty("bodyStyleName")]
        public string BodyStyleName { get; set; }
    }
}
