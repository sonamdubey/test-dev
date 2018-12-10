using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Dealers;
using System.Collections.Generic;
using Carwale.Entity.Classified;
using Carwale.Entity.UserProfiling;
using Carwale.Entity.CMS.UserReviews;
using Carwale.DTOs.CarData;
using Carwale.Entity.ViewModels.CarData;
using Carwale.Entity.CMS;
namespace Carwale.DTOs.NewCars
{
    public class MakePageDTO_Desktop
    {
        public List<CarModelSummaryDTOV2> NewCarModels { get; set; }
        public List<CarModelSummary> DiscontinuedCarModels { get; set; }
        public List<UserReviewEntity> MakeUserReviews { get; set; }
        public CarMakeEntityBase MakeDetails { get; set; }
        public List<ArticleSummary> ExpertReviews { get; set; }
        public List<ArticleSummary> News { get; set; }
        public CarMakeDescription CarMakeDescription { get; set; }
        public List<UpcomingCarModel> UpcomingModels { get; set; }
        public List<DealerBrandingProperty> SponsoredDealerProperty { get; set; }
        public List<CarModelEntityBase> CarModels_QuickResearch { get; set; }
        public List<CarMakeEntityBase> CarMakes_QuickResearch { get; set; }
        public List<StockSummary> UsedLuxuryCars { get; set; }
        public List<DealerCityEntity> LocateDealerCities { get; set; }
        public List<Video> MakeVideos { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public ImageCarousal Images { get; set; }
    }
}
