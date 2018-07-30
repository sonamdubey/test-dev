using Newtonsoft.Json;

namespace Bikewale.DTO.Model.v3
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 12 Apr 2016
    /// Description :   This new DTO for model API photos
    /// </summary
    public class CMSModelImageBase
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}
