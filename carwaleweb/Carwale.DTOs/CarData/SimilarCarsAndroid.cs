using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    /// <summary>
    /// Alternate(similar) cars For Android 
    /// Written By : Ashish Verma on 2/6/2014
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public class SimilarCarsAndroidDTO
    {
        public string largePicUrl { get; set; }
        public double minPrice { get; set; }
        public double maxPrice { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int modelId { get; set; }
        public decimal reviewRate { get; set; }
        public int reviewCount { get; set; }
        public string smallPicUrl { get; set; }
        public string carModelUrl { get; set; }
        public int versionId { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}
