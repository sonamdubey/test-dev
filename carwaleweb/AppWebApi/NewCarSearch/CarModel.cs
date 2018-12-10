using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;

namespace NewCarSearch
{
    /* Author: Rakesh Yadav
     * Date Created: 14 June 2013
     */
    public class CarModel
    {
        public string SmallPic { get; set; }
        public string LargePicUrl { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaxPrice { get; set; }
        public string MinPrice { get; set; }
        public string CarRating { get; set; }
        public string CarModelUrl { get; set; }
        public string CarPrice { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
    }

    public class CarModelV2
    {
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
        [JsonProperty("modelId")]
        public string ModelId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("carRating")]
        public string CarRating { get; set; }
        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }
    }
}