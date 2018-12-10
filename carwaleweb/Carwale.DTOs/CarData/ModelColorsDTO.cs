using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
   public class ModelColorsDTO
    {
        [JsonProperty("name")]
        public string Color;

        [JsonProperty("code")]
        public string HexCode;
    }
}
