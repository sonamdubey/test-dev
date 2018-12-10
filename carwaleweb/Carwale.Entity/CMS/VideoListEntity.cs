using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{
    /// <summary>
    /// Author      :   Sumit Kate on 18 Feb 2016
    /// Description :   Videos List
    /// </summary>
    [Serializable, JsonObject]
    public class VideoListEntity
    {
        [JsonProperty]
        public List<Video> Videos { get; set; }

        [JsonProperty]
        public int TotalRecords { get; set; }

        [JsonProperty]
        public string PrevPageUrl { get; set; }

        [JsonProperty]
        public string NextPageUrl { get; set; }
    }
}
