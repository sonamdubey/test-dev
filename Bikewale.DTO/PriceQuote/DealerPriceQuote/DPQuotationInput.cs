using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer price quote input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DPQuotationInput
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
    }
}
