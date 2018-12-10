using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarColorDTO
    {
        [JsonProperty("id")]
        public int ColorId
        {
            get;
            set;
        }
        [JsonProperty("name")]
        public string ColorName
        {
            get;
            set;
        }
        [JsonProperty("hexCode")]
        public string HexCode
        {
            get;
            set;
        }
    }
}

