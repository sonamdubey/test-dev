using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarValuation
{
    public class ValuationRecommendationDTO
    {
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("year")]
        public string MakeYear { get; set; }

        [JsonProperty("kms")]
        public string Km { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("carName")]
        public string CarName { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("smallPicUrl")]
        public string ImageUrlSmall { get; set; }

        [JsonProperty("usedCarDetail")]
        public string CarDetailUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string ImageUrlMedium { get; set; }

        [JsonProperty("isPremium")]
        public string IsPremium { get; set; }

        [JsonProperty("updated")]
        public string LastUpdatedOn { get; set; }

        [JsonProperty("fuel")]
        public string Fuel { get; set; }

        [JsonProperty("gearBox")]
        public string GearBox { get; set; }

        [JsonProperty("certificationScore")]
        public string CertificationScore { get; set; }
    }
}
