using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>   
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Weighted Doc, Inherit all your docs from this class
    /// </summary>
    public class WeightedDocument: Document
    {
        [JsonProperty("weight")]
        public uint Weight { get; set; }
    }
}
