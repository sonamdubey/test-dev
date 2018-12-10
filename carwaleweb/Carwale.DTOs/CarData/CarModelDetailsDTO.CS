using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{

    /// <summary>
    /// Created By : Ajay Singh on 1 march 2016
    /// </summary>
    /// 
    public class CarModelDetailsDTO : CarModelSummaryAppDTO
    {
    
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonIgnore]
        [JsonProperty("smallPicUrl")]
        public string ModelImageSmall { get; set; }

        [JsonIgnore]
        [JsonProperty("largePicUrl")]
        public string ModelImageLarge { get; set; }

        [JsonIgnore]
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonIgnore]
        [JsonProperty("used")]
        public int Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonIgnore]
        [JsonProperty("rootId")]
        public int RootId { get; set; }

        [JsonIgnore]
        [JsonProperty("videoCount")]
        public int VideoCount { get; set; }

        [JsonIgnore]
        [JsonProperty("carCount")]
        public int CarCount { get; set; }

        [JsonIgnore]
        [JsonProperty("originalImg")]
        public string OriginalImgPath { get; set; }

        [JsonIgnore]
        [JsonProperty("bodyStyleId")]
        public int BodyStyleId { get; set; }

        [JsonIgnore]
        [JsonProperty("subSegmentId")]
        public byte SubSegmentId { get; set; }

        [JsonIgnore]
        [JsonProperty("modelPopularity")]
        public int ModelPopularity { get; set; }
     
        [JsonProperty("minPrice")]
        public string MinPriceNew { get; set; }
       
        [JsonProperty("maxPrice")]
        public string MaxPriceNew { get; set; }

        [JsonProperty("exShowroomCity")]
        public string ExShowroomCity { get; set; }

        [JsonProperty("exShowroomCityId")]
        public string ExShowroomCityId { get; set; }


        [JsonProperty("isDiscontinuedCar")]
        public bool IsDiscontinuedCar { get; set; }

        [JsonProperty("shareUrl")]
        public string shareUrl { get; set; }
    }
  
   
}
