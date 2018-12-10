using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /*
         Author:Rakesh Yadav
         Date Created: 16 july 2013
    */
    public class Review
    {
        [JsonProperty("reviewId")]
        public int ReviewId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("reviewDate")]
        public string ReviewDate { get; set; }
        [JsonProperty("reviewRate")]
        public string ReviewRate { get; set; }
        [JsonProperty("goods")]
        public string Goods { get; set; }
        [JsonProperty("bads")]
        public string Bads { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("reviewUrl")]
        public string ReviewUrl { get; set; }
        [JsonProperty("handleName")]
        public string HandleName { get; set; }
        [JsonProperty("threadId")]
        public int ThreadId { get; set; }
        [JsonProperty("comments")]
        public int Comments { get; set; }
    }
}