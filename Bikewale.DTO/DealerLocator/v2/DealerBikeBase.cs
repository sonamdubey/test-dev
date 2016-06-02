
using Newtonsoft.Json;
namespace Bikewale.DTO.DealerLocator.v2
{
    /// <summary>
    /// Created by  :   Sumit Kate on 20 May 2016
    /// Description :   Bike base dto
    /// </summary>
    public class DealerBikeBase
    {
        [JsonProperty("versionName")]
        public string Bike { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
    }
}
