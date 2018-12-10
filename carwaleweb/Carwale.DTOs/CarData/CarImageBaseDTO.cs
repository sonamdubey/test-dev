using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarImageBaseDTO
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}
