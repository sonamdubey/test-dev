using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    /// <summary>
    /// Created By : Satish on 3 Nov 2015
    /// </summary>
    public class PriceDTO
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("categoryItemId")]
        public int CategoryItemId { get; set; }
        [JsonProperty("categoryItemName")]
        public string CategoryItemName { get; set; }
        [JsonProperty("categoryItemValue")]
        public int CategoryItemValue { get; set; }
        [JsonProperty("isMetallic")]
        public bool IsMetallic { get; set; }
        [JsonProperty("categoryType")]
        public int CategoryType { get; set; }
    }
}
