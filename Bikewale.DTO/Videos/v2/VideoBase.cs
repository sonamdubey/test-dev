using Newtonsoft.Json;

namespace Bikewale.DTO.Videos.v2
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 9th Oct 2017
    /// </summary>
    public class VideoBase
    {
        [JsonProperty("videoTitle")]
        public string VideoTitle { get; set; }
        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
        [JsonProperty("views")]
        public uint Views { get; set; }
        [JsonProperty("likes")]
        public uint Likes { get; set; }
        [JsonProperty("videoTitleUrl")]
        public string VideoTitleUrl { get; set; }
        [JsonProperty("displayDate")]
        public string DisplayDate { get; set; }
    }
}
