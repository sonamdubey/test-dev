using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarModelDetailsDTO_V1
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        
        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("isDiscontinuedCar")]
        public bool IsDiscontinuedCar { get; set; }        

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImage { get; set; }

        [JsonProperty("offerExists")]
        public bool OfferExists { get; set; }

        [JsonProperty("reviewRate")]
        public string ModelRating { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }
        [JsonProperty("threeSixtyAvailability")]
        public ThreeSixtyAvailabilityDTO ThreeSixtyAvailability { get; set; }
    }
}
