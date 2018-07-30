using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Mobile verified output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DPQMobileVerifiedOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
    }
}
