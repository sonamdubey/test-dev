using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /*
    Author: Rakesh Yadav
    Date created: 3 Oct 2013
    */
    public class SortCriteria
    {
        [JsonProperty("sortText")]
        public string SortText { get; set; }
        [JsonProperty("sortFor")]
        public string SortFor { get; set; }
        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
        [JsonProperty("sortUrl")]
        public string SortUrl { get; set; }
    }
}