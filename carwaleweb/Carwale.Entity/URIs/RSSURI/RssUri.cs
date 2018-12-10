using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.URIs.RSSURI
{
    public class RssUri
    {
        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }

        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }

        [JsonProperty(PropertyName = "days")]
        public uint Days { get; set; }
    }
}
