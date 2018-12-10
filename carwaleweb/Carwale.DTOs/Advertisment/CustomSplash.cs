using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Advertisment
{
    public class CustomSplashDTO
    {
        [JsonProperty("splashImgUrl")]
        public string SplashImgUrl { get; set; }
    }
}
