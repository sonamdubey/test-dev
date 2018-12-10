using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class TipsAndAdvicesCategory
    {
        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         */
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

    }
}