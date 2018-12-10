using Newtonsoft.Json;

namespace Carwale.DTOs.Stock
{
    public class StockApp
    {
        public string ProfileId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string Price { get; set; }
        public string Year { get; set; }
        public string Kms { get; set; }
        public string Fuel { get; set; }
        public string GearBox { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public int DeliveryCityId { get; set; }
        public string DeliveryText { get; set; }
        public string CertificationScore { get; set; }
        public string ShareUrl { get; set; }
        public string FinanceEmi { get; set; }
        public string FinanceLinkText { get; set; }
        public string FinanceUrl { get; set; }
        public SectionsAvailable SectionsAvailable { get; set; }
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        [JsonProperty("usedCarCityId")]
        public uint CityId { get; set; }
        public string ValuationUrl { get; set; }
        public string ValuationText { get; set; }
        public string DealerRatingText { get; set; }
        public bool IsChatAvailable { get; set; }
    }
}
