using Carwale.Entity.Geolocation;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class NearByCityDetailsDto
    {
        [JsonProperty("cities")]
        public List<NearByCityDto> Cities { get; }

        [JsonProperty("widgetHeading")]
        public string WidgetHeading { get; set; }

        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("isCrossSellPriceQuote")]
        public bool IsCrossSellPriceQuote { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("cardNo")]
        public int CardNo { get; set; }

        public NearByCityDetailsDto()
        {
            Cities = new List<NearByCityDto>();
        }
    }
}
