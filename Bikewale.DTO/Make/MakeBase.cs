using Newtonsoft.Json;

namespace Bikewale.DTO.Make
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Summary : HostUrl and LogoUrl Added
    /// </summary>
    public class MakeBase
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }        
    }
}
