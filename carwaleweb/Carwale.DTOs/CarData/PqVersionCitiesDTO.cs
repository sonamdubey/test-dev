using Carwale.DTOs.Autocomplete;
using Carwale.DTOs.Geolocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class PqVersionCitiesDTO
    {
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }
        
        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }
        
        [JsonProperty("minPrice")]
        public string MinPrice { get; set; }
        
        [JsonProperty("maxPrice")]
        public string MaxPrice { get; set; }
        
        [JsonProperty("carPrice")]
        public string CarPrice { get; set; }

        [JsonProperty("reviewRate")]
        public string ReviewRate { get; set; }

        [JsonProperty("OfferExists")]
        public bool OfferExists { get; set; }

        [JsonProperty("reviewCount")]
        public string ReviewCount { get; set; }

        [JsonProperty("exShowroomCity")]
        public string ExShowroomCity { get; set; }

        [JsonProperty("carName")]
        public string CarName { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("versions")]
        public List<LabelValueDTO> Versions { get; set; }
        
        [JsonProperty("cities")]
        public List<LabelValueDTO> Cities { get; set; }

        [JsonProperty("zones")]
        public List<CityZonesDTO> Zones { get; set; }
    }
}
