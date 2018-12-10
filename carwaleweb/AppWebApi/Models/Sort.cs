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
    public class Sort
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("sortUrl")]
        public string SortUrl { get; set; }
    }
}