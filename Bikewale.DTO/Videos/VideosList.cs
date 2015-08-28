using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Videos
{
    public class VideosList
    {
        [JsonProperty("videos")]
        public IEnumerable<VideoBase> Videos { get; set; }
    }
}
