using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Videos
{
    public class VideosList
    {
        [JsonProperty("videos")]
        public IEnumerable<VideoBase> Videos { get; set; }
    }
}
