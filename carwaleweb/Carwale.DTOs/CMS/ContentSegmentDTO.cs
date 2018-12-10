using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class ContentSegmentDTO
    {
        [JsonProperty("categoryId")]
        public int CategoryId {get;set;}
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("count",NullValueHandling=NullValueHandling.Ignore)]
        public int RecordCount { get; set; }
    }
}
