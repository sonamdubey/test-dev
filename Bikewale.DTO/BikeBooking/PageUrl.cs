using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking
{
    public class PagingUrl
    {
        [JsonProperty("prevPageUrl")]
        public string PrevPageUrl { get; set; }
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }
}
