using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Insurance
{
    public class ClientResponse
    {
        [JsonProperty("quotation")]
        public string Quotation { get; set; }

        [JsonProperty("confirmationStatus")]
        public string ConfirmationStatus { get; set; }

        [JsonProperty("uniqueId")]
        public int UniqueId { get; set; }
    }
}
