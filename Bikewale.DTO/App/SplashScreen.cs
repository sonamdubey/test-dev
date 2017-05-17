
using Newtonsoft.Json;
namespace Bikewale.DTO
{
    public class SplashScreen
    {
        [JsonProperty("splashImgUrl")]
        public string SplashImgUrl { get; set; }

        [JsonProperty("splashTimeOut")]
        public string SplashTimeOut { get; set; }
    }
}
