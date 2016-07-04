using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote
{

    /// <summary>
    /// Created By :  Sushil Kumar
    /// Created On  : 17th June 2016
    /// Description : Version price for secondary dealers with version prices
    /// </summary>
    public class VersionPriceBase
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("price")]
        public uint VersionPrice { get; set; }
    }
}
