using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarMakeLogo
    {
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
    }
}
