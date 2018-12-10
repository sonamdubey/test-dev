using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class HTMLItem
    {
        /*
         Author:Rakesh Yadav 
         Date Created: 1 Oct 2013
         */
        
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("setMargin")]
        public bool SetMargin { get; set; }

        [JsonProperty("contentList")]
        public List<string> ContentList { get; set; }
    }
}