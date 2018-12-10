using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /*
     Author:Rakesh Yadav
     Date Created: 30 July 2013
     */
    public class NewsItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("pubDate")]
        public string PubDate { get; set; }
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }
        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }
        [JsonProperty("largepPicUrl")]
        public string LargePicUrl { get; set; }
        [JsonProperty("mediumPicUrl")]
        public string MediumPicUrl { get; set; }
        [JsonProperty("cwNewsDetailUrl")]
        public string CWNewsDetailUrl { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonIgnore]
        public ulong BasicId { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
    }
}