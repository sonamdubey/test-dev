
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.AWS
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   AWS Token DTO
    /// </summary>
    public class Token
    {
        [Required, JsonProperty("key")]
        public string Key { get; set; }
        [Required, JsonProperty("uri")]
        public string URI { get; set; }
        [Required, JsonProperty("accessKeyId")]
        public string AccessKeyId { get; set; }
        [Required, JsonProperty("policy")]
        public string Policy { get; set; }
        [Required, JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("datetimeiso")]
        public string DatetTmeISO { get; set; }
        [JsonProperty("datetimeisolong")]
        public string DateTimeISOLong { get; set; }
    }
}
