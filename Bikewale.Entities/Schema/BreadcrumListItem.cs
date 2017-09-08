using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumListItem : ListItem
    {
        [JsonProperty("item", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumItem Item { get; set; } 
    }
}
