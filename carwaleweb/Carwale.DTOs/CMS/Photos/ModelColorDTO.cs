using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Photos
{
    public class ModelColorsDTO
    {
        [JsonProperty("colorId")]
        public int ColorId { get; set; }

        [JsonProperty("name")]
        public string ColorName { get; set; }
        
        [JsonProperty("hexCodes")]
        public List<string> HexCode { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}
