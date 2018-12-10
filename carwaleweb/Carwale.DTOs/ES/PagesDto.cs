using Carwale.DTOs.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.ES
{
    public class PagesDto : IdNameDto
    {        
        [JsonProperty("properties")]
        public List<PropertiesDto> Properties { get; set; }
    }   
}