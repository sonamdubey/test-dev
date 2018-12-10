using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class ValidationErrors
    {
        public string Description { get; set; }
    }
    public class C2BStockApiResponse
    {
        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("errorcode")]
        public short ErrorCode { get; set; }

        [JsonProperty("c2b_intermediate_id")]
        public int IntermediateId { get; set; }

        [JsonProperty("details")]
        public List<ValidationErrors> Details { get; set; }
    }
}
