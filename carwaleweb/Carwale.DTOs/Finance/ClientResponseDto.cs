using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Finance
{
    public class ClientResponseDto
    {
        [JsonProperty("exitPointUrl")]
        public string ExitPointUrl { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("cromaNewLeadId")]
        public long CromaNewLeadId { get; set; }

        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
