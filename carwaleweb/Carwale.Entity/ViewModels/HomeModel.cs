using Carwale.Entity.CarData;
using Carwale.Entity.Classified.PopularUsedCars;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Deals;
using Carwale.Entity.Geolocation;
using Carwale.Entity.IPToLocation;
using System.Collections.Generic;

namespace Carwale.Entity.ViewModels
{
    public class HomeModel
    {
        public List<UpcomingCarModel> Upcoming { get; set; }
        public List<LaunchedCarModel> NewLaunches { get; set; }
        public List<HotCarComparison> HotComparisons { get; set; }
        public List<ArticleSummary> TopNews { get; set; }
        public List<ArticleSummary> TopReviews { get; set; }
        public List<Video> TopVideos { get; set; }
        public List<PopularUsedCarModel> PopularUsedCars { get; set; }
        public List<DealsStock> CarsWithBestSavings { get; set; }
        public Sponsored_Car SponsoredHomeBanner { get; set; }
        public IPToLocationEntity IPToLocation { get; set; }
        public City UsedCity { get; set; }
        public Sponsored_Car NewCarPlaceHolder { get; set; }
        public Sponsored_Car PQPlaceHolder { get; set; }
        public TopSellingModel TopSellingModel { get; set; }
        public Sponsored_Car SearchExampleText { get; set; }
        public Sponsored_Car SponsoredPQBanner { get; set; }
    }
}