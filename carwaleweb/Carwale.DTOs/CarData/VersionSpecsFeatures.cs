using Carwale.DTOs.Autocomplete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class VersionSpecsFeatures
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("categoryData")]
        public List<LabelValueDTO> CategoryData { get; set; }
    }
}
