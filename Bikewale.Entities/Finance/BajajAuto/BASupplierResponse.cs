using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Finance.BajajAuto
{

    public class BASupplierResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("responseMsg")]
        public string ResponseMsg { get; set; }
        [JsonProperty("supplierDetails")]
        public IEnumerable<BASupplierDetails> SupplierDetails { get; set; }
    }
    public class BASupplierDetails
    {
        [JsonProperty("sup_name")]
        public string SupplierName { get; set; }
        [JsonProperty("icas_supplierid")]
        public string IcasSupplierId { get; set; }
    }
}
