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
    public class ReviewVersions
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("reviewUrl")]
        public string ReviewUrl { get; set; }
    }
}