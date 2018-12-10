using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class VersionSpecsFeaturesTab
    {
        [JsonProperty("categories")]
        public List<VersionSpecsFeatures> Categories = new List<VersionSpecsFeatures>();
    }
}
