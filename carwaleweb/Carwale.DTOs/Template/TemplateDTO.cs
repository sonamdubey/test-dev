using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Template
{
    public class TemplateDTO
    {
        [JsonProperty("html")]
        public string HTML { get; set; }
    }
}
