using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs
{
    public class AppResponse
    {
        [JsonProperty("responseCode")]
        public string Code { get; set; }
        [JsonProperty("responseMessage")]
        public string Message { get; set; }
    }


    public class AppResponseV2
    {
        [JsonProperty("responseCode")]
        public string Code { get; set; }
        [JsonProperty("responseMessage")]
        public string Message { get; set; }
    }
}
