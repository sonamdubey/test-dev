using System;

namespace Carwale.Entity
{
    public class SearchResult
    {
        public string ProfileId { get; set; }
        public CarEntity Car { get; set; }
        public string MaskingName { get; set; }
        public int MaskingId { get; set; }
        public UInt64 Price { get; set; }
        public int Kms { get; set; }
        public DateTime  MakeYear { get; set; }
        public string FuelType { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; }
        public string SellerType { get; set; }
        public string OwnerType { get; set; }
        public int PhotoCount { get; set; }
        public string ThumbImageUrl { get; set; }
        public string CertifiedLogoUrl { get; set; }
        public bool IsPremiumAd { get; set; }
        public bool IsVideoAvalable { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AreaName { get; set; }
        public string AreaCityName { get; set; }
        public string ProfilePageUrl { get; set; }
        public DateTime OfferStartDate { get; set; }
        public DateTime OfferEndDate { get; set; }
    }
}
