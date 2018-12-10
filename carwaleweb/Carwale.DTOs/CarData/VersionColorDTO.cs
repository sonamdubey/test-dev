using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarColorsDto
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("colors")]
        public List<VersionColorDto> Colors { get; set; }
    }
    public class VersionColorDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hexCodes")]
        public List<string> HexCodes { get; set; }
    }
}
