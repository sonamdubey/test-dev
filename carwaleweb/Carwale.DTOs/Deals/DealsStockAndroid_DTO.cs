using Newtonsoft.Json;
using System.Collections.Generic;
using Carwale.DTOs.Geolocation;

namespace Carwale.DTOs.Deals
{
    public class DealsStockAndroid_DTO
    {
        [JsonProperty("manufacturingYear")]
        public int ManufacturingYear { get; set; }
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("onRoadPrice")]
        public int OnRoadPrice { get; set; }
        [JsonProperty("offerPrice")]
        public int OfferPrice { get; set; }
        [JsonProperty("savings")]
        public int Savings { get; set; }
        [JsonProperty("stockCount")]
        public int StockCount { get; set; }
        [JsonProperty("offers")]
        public string Offers { get; set; }
        [JsonProperty("termsConditions")]
        public string TermsConditions { get; set; }
        [JsonProperty("stockId")]
        public int StockId { get; set; }
        [JsonProperty("selectedYear")]
        public int SelectedYear { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
        [JsonProperty("showExtraSavings")]
        public bool ShowExtraSavings { get; set; }
        [JsonProperty("extraSavings")]
        public int ExtraSavings { get; set; }
        [JsonProperty("bookingReasons")]
        public BookingReasons ReasonsSlug { get; set; }
        [JsonProperty("offerValue")]
        public string OfferValue { get; set; }
        [JsonProperty("isBreakUpAvailable")]
        public bool IsBreakUpAvailable { get; set; }
        [JsonProperty("priceBreakupId")]
        public int PriceBreakUpId { get; set; }
        [JsonProperty("dealsEmiDetails")]
        public DealsEmiDTO DealsEmiDetails { get; set; }
        [JsonProperty("deliveryTimeline")]
        public int DeliveryTimeline { get; set; }
        [JsonProperty("dealsOfferList")]
        public List<KeyValuePair<string, string>> OfferList { get; set; }
        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }
        [JsonProperty("dealerCity")]
        public City City { get; set; }
        [JsonProperty("dealerArea")]
        public AreaDto Area { get; set; }
    }
}
