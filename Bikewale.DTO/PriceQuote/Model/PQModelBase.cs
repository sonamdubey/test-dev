using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.Model
{
    /// <summary>
    /// Price Quote Model base
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQModelBase
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
