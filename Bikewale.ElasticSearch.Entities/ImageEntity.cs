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
    /// Description: Bike Image
    /// </summary>
    public class ImageEntity
    {
        [JsonProperty("hostURL")]
        public string HostURL { get; set; }
        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }

    }
}
