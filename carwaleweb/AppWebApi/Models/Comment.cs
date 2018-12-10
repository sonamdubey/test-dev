using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class Comment
    {
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("pubDate")]
        public string Pubdate { get; set; }
        [JsonProperty("content")]
        public string Content {get;set;}
    }
}