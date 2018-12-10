using System.Configuration;

namespace Carwale.Utility.Classified
{
    public static class Constants
    {
        #region search
        public static int WebAndMobileRecommendationCount { get; } = 25;
        public static int AppRecommendationCount { get; } = 6;
        public static double PriceInterval { get; } = 0.25d;
        public static int IterationCount { get; } = 3;
        public static string TrackingCatForRecommendations { get; } = "UsedStockRecommendations";
        public static string TrackingActionForRecommendations { get; } = "RecommendationCount";
        public static int FranchiseCarsPackageId { get; } = 1;
        public static int MaxLimitForFranchiseCars { get; } = 30;
        public static string ClassifiedElasticIndex { get; } = ConfigurationManager.AppSettings["ElasticIndexName"];
        public static string ClassifiedElasticIndexType { get; } = "stock";
        public static string FranchiseLogoUrl { get; } = "https://imgd.aeplcdn.com/0x0/cw/static/used/franchisee-logo.svg?=v2";
        public static int DefaultCityIdForFeaturedSlots { get; } = 10000;
        public static string C_getCarWithPhotos { get; } = "1";
        public static string AllIndiaCityId { get; } = "9999";
        public static int DiamondDealerPackageType { get; } = 54;
        public static string[] DealerPackageExcluded { get; }= { "18" };
        public static string[] PaidIndPackageTypes { get; }= { "58" };
        public static int CarsNearMeBucketRange { get; } = 8;
        public static int DefaultPageSize { get; } = 24;
        public static string FranchiseeStocksSearchBucket { get; } = "franchisee";
        public static string DiamondStocksSearchBucket { get; } = "diamond";
        public static string PlatinumStocksSearchBucket { get; } = "platinum";
        public static string ESClassifiedStockType { get; } = "stock";
        public static string NonFeaturedStocksSearchBucket { get; } = "nonFeatured";
        public static string CarTradeCertificationId { get; } = ConfigurationManager.AppSettings["CartradeCertificationId"];
        #endregion

        #region sellcar
        public static int IndividualListingLimit { get { return 1; } }
        public static string IndividualListingLimitMessage { get { return @"You have reached the maximum listing limit on CarWale.
                    If you wish to add new listing, you can delete the existing one by logging from desktop site and then create a new one."; } }
        #endregion
    }
}
