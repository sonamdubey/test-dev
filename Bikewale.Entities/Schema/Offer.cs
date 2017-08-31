using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To add offer properties to product or a thing (mainly used for pricing purposes)
    /// </summary>
    public class Offer
    {
        /* Commnted by Sangram on 31 Aug 2017, Use AgreegateOffer instead*/

        //[JsonProperty("@type")]
        //public string Type { get { return "Offer"; } }
        
        //[JsonProperty("availability", NullValueHandling = NullValueHandling.Ignore)]
        //public string Availability { get; set; }
        
        //[JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        //public double Price { get; set; }

        //[JsonProperty("priceCurrency")]
        //public string PriceCurrency { get { return "INR"; } }

        //[JsonProperty("lowPrice", NullValueHandling = NullValueHandling.Ignore)]
        //private string LowPrice { get; set; }

        //[JsonProperty("highPrice", NullValueHandling = NullValueHandling.Ignore)]
        //public string HighPrice { get; set; }
    }
}
