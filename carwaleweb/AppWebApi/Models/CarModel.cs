using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    /* Author: Rakesh Yadav
     * Date Created: 12 June 2013
     * Discription: Create Model CarModel */
    public class CarModel
    {
        [JsonProperty("smallPicUrl")]
        public string SmallPic { get; set; }
        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
        [JsonProperty("modelID")]
        public string ModelId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maxPrice")]
        public string MaxPrice { get; set; }
        [JsonProperty("minPrice")]
        public string MinPrice { get; set; }
        [JsonProperty("carRating")]
        public string CarRating { get; set; }
        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }
        [JsonProperty("carPrice")]
        public string CarPrice { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}