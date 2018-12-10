using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /*
     Author:Rakesh Yadav
     Date Created: 30 July 2013
     */
    public class UpCommingCarItem
    {
        [JsonProperty("carName")]
        public string CarName { get; set; }
        [JsonProperty("smallpicUrl")]
        public string SmallpicUrl { get; set; }
        [JsonProperty("estimatedPrice")]
        public string EstimatedPrice { get; set; }
        [JsonProperty("expectedLaunchDate")]
        public string ExpectedLaunchDate { get; set; }
        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }
    }
}