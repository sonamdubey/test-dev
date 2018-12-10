using Carwale.DTOs.Geolocation;
using Newtonsoft.Json;

namespace Carwale.DTOs.PriceQuote
{
    public class PriceOverviewDTO
    {
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("priceLabel")]
        public string PriceLabel { get; set; }
        [JsonProperty("priceSuffix")]
        public string PriceSuffix { get; set; }
        [JsonProperty("pricePrefix")]
        public string PricePrefix { get; set; }
        [JsonProperty("labelColor")]
        public string LabelColor { get; set; }
        [JsonProperty("reasonText")]
        public string ReasonText { get; set; }
        [JsonProperty("city")]
        public CityDTO City { get; set; }
        [JsonProperty("cityColor")]
        public string CityColor { get; set; }
        [JsonProperty("priceStatus")]
        public int PriceStatus { get; set; }
        [JsonIgnore]
        public int PriceForSorting { get; set; }
        [JsonProperty("formattedFullPrice")]
        public string FormattedFullPrice { get; set; }
    }  

    /*WE ARE USING THIS DTO FOR DESKTOP AND MSITE.WE NEED THIS BECAUSE 
    WE NEED A DIFFFRENT MAPPING OF PRICEOVERVIEW ENTITY AND PRICEOVERVIEWDTO.
    THERE IS 2 MAPPING IN WEBAPICONFIG FILE FOR PRICEOVERVIEW ENTITY.
    ONE IS USED FOR APP AND OTHER FOR WEB.*/
    public class PriceOverviewDTOV2
    {
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("priceLabel")]
        public string PriceLabel { get; set; }
        [JsonProperty("priceSuffix")]
        public string PriceSuffix { get; set; }
        [JsonProperty("pricePrefix")]
        public string PricePrefix { get; set; }
        [JsonProperty("labelColor")]
        public string LabelColor { get; set; }
        [JsonProperty("reasonText")]
        public string ReasonText { get; set; }
        [JsonProperty("city")]
        public CityDTO City { get; set; }
        [JsonProperty("cityColor")]
        public string CityColor { get; set; }
        [JsonProperty("priceStatus")]
        public int PriceStatus { get; set; }
        [JsonProperty("priceForSorting")]
        public int PriceForSorting { get; set; }
    }

    /// <summary>
    /// Removed city details from price object
    /// </summary>
    public class PriceOverviewDtoV3 
    {
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("priceLabel")]
        public string PriceLabel { get; set; }
        [JsonProperty("priceSuffix")]
        public string PriceSuffix { get; set; }
        [JsonProperty("pricePrefix")]
        public string PricePrefix { get; set; }
        [JsonProperty("labelColor")]
        public string LabelColor { get; set; }
        [JsonProperty("reasonText")]
        public string ReasonText { get; set; }
        [JsonProperty("priceStatus")]
        public int PriceStatus { get; set; }
        [JsonIgnore]
        public int PriceForSorting { get; set; }
        [JsonProperty("formattedFullPrice")]
        public string FormattedFullPrice { get; set; }
    }  
}

