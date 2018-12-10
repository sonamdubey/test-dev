using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable, JsonObject]
    public class CarLaunchDiscontinueYear
    {
       [JsonProperty]
        public int LaunchYear { get; set; }

        [JsonProperty]
        public int DiscontinueYear { get; set; }
    }
}
