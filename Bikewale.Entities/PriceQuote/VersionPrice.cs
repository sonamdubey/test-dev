using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By :  Sushil Kumar
    /// Created On  : 17th June 2016
    /// Description : Version price for secondary dealers with version prices
    /// </summary>
    public class VersionPriceEntity
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("versionPrice")]
        public uint VersionPrice { get; set; }
    }
}
