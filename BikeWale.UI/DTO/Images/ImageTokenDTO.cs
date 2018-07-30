using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image token DTO
    /// </summary>
    public class ImageTokenDTO : AWS.Token
    {
        [Required, JsonProperty("id")]
        public uint? Id { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        [JsonIgnore]
        public bool ServerError { get; set; }
        [JsonProperty("photoId")]
        public uint? PhotoId { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
    }
}
