using Carwale.DTOs.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PageProperty
{
    public class PagePropertyDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("template")]
        public TemplateDTO Template { get; set; }
    }
}
