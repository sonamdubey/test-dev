using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class WebPage
    {
        [JsonProperty("@type")]
        public string Type { get { return "WebPage"; } }

        [JsonProperty("breadcrumb", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumList Breadcrum { get; set; }
    }
}
