
using Newtonsoft.Json;
namespace Bikewale.DTO
{
    public class SplashScreen
    {
        [JsonProperty("splashImgUrl")]
        public string SplashImgUrl { get; set; }

        [JsonProperty("splashTimeOut")]
        public uint SplashTimeOut { get; set; }
    }
}
