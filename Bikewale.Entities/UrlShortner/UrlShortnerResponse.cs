using Newtonsoft.Json;

namespace Bikewale.Entities.UrlShortner
{
    public class UrlShortnerResponse
    {
        /// <summary>
        /// Edited by: Sangram Nandkhile 02 Sep 2016.
        /// Summary: Changed the Id property to ShortUrl but kept JSONPROP as it is. It is used for mapping with google's response.
        /// </summary>
        [JsonProperty("id")]
        public string ShortUrl { get; set; }
        [JsonProperty("longUrl")]
        public string LongUrl { get; set; }
    }
}
