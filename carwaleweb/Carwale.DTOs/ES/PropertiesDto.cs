using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.ES
{
    public class PropertiesDto : IdNameDto
    {
        [JsonProperty("pagePropertyMappingId")]
        public int PagePropertyMappingId { get; set; }
    }
}