using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /*
    Author:Rakesh Yadav
    Date Created: 18 july 2013 
    */
    public class ReviewDetail
    {
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
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("purchasedAs ")]
        public string PurchasedAs { get; set; }
        [JsonProperty("fuelEconomy")]
        public string FuelEconomy { get; set; }
        [JsonProperty("familarity")]
        public string Familiarity { get; set; }
    }
}