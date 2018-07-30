using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Videos.v2
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 09th Oct 2017
    /// </summary>
    public class VideosList
    {
        [JsonProperty("videos")]
        public IEnumerable<VideoBase> Videos { get; set; }
    }
}
