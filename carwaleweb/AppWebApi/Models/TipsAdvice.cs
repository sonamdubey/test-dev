using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class TipsAdvice
    {
        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         */
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("pubDate")]
        public string PubDate { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }
    }
}