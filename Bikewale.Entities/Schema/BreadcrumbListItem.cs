using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumbListItem : ListItem
    {
        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumbItem Item { get; set; } 
    }
}
