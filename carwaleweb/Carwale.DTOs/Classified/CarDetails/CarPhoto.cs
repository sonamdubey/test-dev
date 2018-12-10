using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class CarPhoto
    {
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("isMainUrl")]
        public bool IsMainUrl { get; set; }
    }
}
