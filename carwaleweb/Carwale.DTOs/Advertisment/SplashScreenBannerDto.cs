using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Advertisment
{
    public class SplashScreenBannerDto
    {
        [JsonProperty("splashImgUrl")]
        public string Splashurl { get; set; }

        [JsonProperty("adTimeOut")]
        public string AdTimeOut { get; set; }
    }
}
