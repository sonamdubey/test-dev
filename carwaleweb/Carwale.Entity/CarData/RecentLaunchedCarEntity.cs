using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// entity made for recent launched cars
    /// written by Natesh Kumar on 1/10/2014
    /// </summary>
    [Serializable, JsonObject]
    public class RecentLaunchedCarEntity : CarMakeModelEntityBase
    {
        
        [JsonProperty(PropertyName = "launchId")]
        public uint LaunchId { get; set; }

        
        [JsonProperty(PropertyName = "basicId")]
        public uint BasicId { get; set; }

        
        [JsonProperty(PropertyName = "hostUrl")]
        public string HostUrl { get; set; }

        
        [JsonProperty(PropertyName = "launchDate")]
        public DateTime LaunchDate { get; set; }

        
        [JsonProperty(PropertyName = "modelImage")]
        public string ModelImage { get; set; }

        
        [JsonProperty(PropertyName = "smallImage")]
        public string SmallImage { get; set; }

        
        [JsonProperty(PropertyName = "reviewRate")]
        public float ReviewRate { get; set; }

        
        [JsonProperty(PropertyName = "reviewCount")]
        public int ReviewCount { get; set; }

        
        [JsonProperty(PropertyName = "minPrice")]
        public double MinPrice { get; set; }

        
        [JsonProperty(PropertyName = "maxPrice")]
        public double MaxPrice { get; set; }

        
        [JsonProperty(PropertyName = "carName")]
        public string CarName { get; set; }

        
        [JsonProperty(PropertyName = "originalImgPath")]
        public string OriginalImgPath { get; set; }

    }

    
}
