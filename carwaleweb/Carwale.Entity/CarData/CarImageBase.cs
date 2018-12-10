using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarImageBase
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }
    }
}
