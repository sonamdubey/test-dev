using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store specification type and its value.
    /// Modified by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Added units for specifications
    /// </summary>
    public class SpecsEntity
    {
        [JsonProperty("specType")]
        public string SpecType { get; set; }
        [JsonProperty("specValue")]
        public double SpecValue { get; set; }
        [JsonProperty("specUnit")]
        public string SpecUnit { get; set; }
    }
}
