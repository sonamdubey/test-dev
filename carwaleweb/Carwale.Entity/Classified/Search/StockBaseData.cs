using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Enum;

namespace Carwale.Entity.Classified.Search
{
    public class StockBaseData
    {
        public string ProfileId { get; set; }
        public string CarName { get; set; }
        public int RootId { get; set; }
        public string Url { get; set; }
        public string NearbyCityText { get; set; }
        public bool IsNearbyCityListing { get; set; }
        public string ValuationUrl { get; set; }
        public int MakeYear { get; set; }
        public string Price { get; set; }
        public string Km { get; set; }
        public string Fuel { get; set; }
        public string AreaName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string HostUrl { get; set; }
        public string EmiFormatted { get; set; }
        public string FinanceUrl { get; set; }
        public bool IsEligibleForFinance { get; set; }
        public bool IsPremium { get; set; }
        public int CertificationId { get; set; }
        public decimal? CertificationScore { get; set; }
        public string OriginalImgPath { get; set; }
        public string DeliveryText { get; set; } = string.Empty;
        public int DeliveryCity { get; set; }
        public string DealerRatingText { get; set; }
        public int CertProgId { get; set; }
        public string CertProgLogoUrl { get; set; }
        public string SimilarCarsUrl { get; set; }
        public string DealerCarsUrl { get; set; }
        public bool ShouldShowBreaker { get; set; }
        public string BreakerText { get; set; }
        public NearbyCarsBucket NearbyCarsBucket { get; set; }
        public CwBasePackageId CwBasePackageId { get; set; }
        public int CtePackageId { get; set; }
        public int SlotId { get; set; }
        public string AdditionalFuel { get; set; }
    }
}
