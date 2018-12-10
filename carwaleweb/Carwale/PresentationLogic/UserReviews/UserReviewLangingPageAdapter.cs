using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.UesrReview;
using Carwale.Entity.Enum;
using Carwale.Entity.CarData;

namespace Carwale.UI.PresentationLogic.UserReviews
{
    public class UserReviewLangingPageAdapter : IServiceAdapterV2
    {
        private readonly IUserReviewLogic _userReviewLogic;
        public UserReviewLangingPageAdapter(IUserReviewLogic userReviewLogic)
        {
            _userReviewLogic = userReviewLogic;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetUserReviewLangingPageDto<U>(input), typeof(T));
        }
        private List<BreadcrumbEntity> BindBreadCrumb()
        {
            List<Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Text = "User Reviews" });
            return _BreadcrumbEntitylist;
        }
        private PageMetaTags BindMetaTags(UserReviewLanding dto)
        {
            PageMetaTags pageMetaTags = new PageMetaTags();
            pageMetaTags.Title = "User ratings & User reviews on cars in India";
            pageMetaTags.Description = "Read reviews about cars from verified owners. Write a review for a car to help buyers in purchasing a new car." +
                                    " Check out the detailed customer feedbacks and ratings.";
            return pageMetaTags;
        }
        public UserReviewLanding GetUserReviewLangingPageDto<U>(U input)
        {
            UserReviewLanding userReviewDto = new UserReviewLanding();
            userReviewDto.IsMobile = (bool)Convert.ChangeType(input, typeof(U));
            userReviewDto.Breadcrumbs = BindBreadCrumb();
            userReviewDto.PageDetails = _userReviewLogic.GetLandingPageDetails(userReviewDto.IsMobile ? Platform.CarwaleMobile : Platform.CarwaleDesktop);
            userReviewDto.MetaTags = BindMetaTags(userReviewDto);
            return userReviewDto;
        }
    }
}