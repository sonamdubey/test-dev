using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    public class PriceEntity
    {
        [JsonProperty("priceType")]
        public string PriceType { get; set; }
        [JsonProperty("priceValue")]
        public uint PriceValue { get; set; }
    }
}
