using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.UserReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.UserReviews
{
    public class UserReviewLanding
    {
        public List<UserReviewDTO> MostReadReviews { get; set; }
        public List<UserReviewDTO> MostHelpfulReviews { get; set; }
        public List<UserReviewDTO> MostRecentReviews { get; set; }
        public List<UserReviewDTO> MostRatedReviews { get; set; }
        public List<CarReviewBaseEntity> MostReviewdCars { get; set; }
        public List<CarMakeEntityBase> Makes { get; set; }
        public List<CarMakeEntityBase> NewMakes { get; set; }
        public List<BreadcrumbEntity> Breadcrumbs { get; set; }
        public bool IsMobile { set; get; }
        public UserReveiwLandingPageDetails PageDetails { get; set; }
        public PageMetaTags MetaTags { get; set; }
    }
}
