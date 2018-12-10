using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini on 30/10/14
    /// </summary>
    public class MileagePageDTO
    {
        public CarModelDetails ModelDetails { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public UpcomingCarModel UpcomingCarDetails { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public ModelMenuDTO ModelMenu { get; set; }
        public List<MileageDataEntity> MileageData { get; set; }
        public CarSynopsisEntity CarSynopsis { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public SponsoredDealer SponsoredDealerAd { get; set; }
        public JObject Schema { get;set; }
    }
}
