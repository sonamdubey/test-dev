namespace Carwale.DTOs.Stock.SimiliarCars
{
    public class StockSummaryDTO
    {
        public string ProfileId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string OriginalImgPath { get; set; }
        public string Price { get; set; }
        public string MakeYear { get; set; }
        public string Kms { get; set; }
        public string Fuel { get; set; }
        public string GearBox { get; set; }
        public string AreaName { get; set; }
        public string CityName { get; set; }
        public decimal? CertificationScore { get; set; }
        public string Color { get; set; }
        public string Url { get; set; }
        public int Rank { get; set; }
        public int CityId { get; set; }
        public string SellerName { get; set; }
        public string Seller { get; set; } //Individual
        public string CarName { get; set; }
        public string HostUrl { get; set; }
        public string Km { get; set; }
        public string DealerRatingText { get; set; }
        public string RootId { get; set; }
        public string BodyStyleId { get; set; }
        public string VersionSubSegmentID { get; set; }
        public string PriceNumeric { get; set; }
        public string KmNumeric { get; set; }
        public string MakeId { get; set; }
        public int PhotoCount { get; set; }
        public bool IsChatAvailable { get; set; }
        public int CtePackageId { get; set; }
        public int SlotId { get; set; }
    }
}
