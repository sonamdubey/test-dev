using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.DTOs.CMS.Models
{
    public class UserReviewsListDTO
    {
        public List<UserReviewEntity> UserReviewsList { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public SubNavigationDTO QuickMenuDetails { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public List<CarVersions> CarVersions { get; set; }
        public List<CarMakeEntityBase> MakeList { get; set; }
        public int PageNumber { get; set; }
        public List<int> PageList { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
    }
}
