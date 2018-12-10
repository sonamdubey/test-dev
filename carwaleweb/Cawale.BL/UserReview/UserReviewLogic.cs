using Carwale.DTOs.UserReviews;
using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Entity.UserReview;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Interfaces.UesrReview;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Carwale.DTOs.CMS.UserReviews;
using AEPLCore.Queue;
using Carwale.Entity.CarData;
using System.Configuration;
using System.Collections.Generic;
using System;


namespace Carwale.BL.UserReview
{
    public class UserReviewLogic : IUserReviewLogic
    {
        private readonly IUserReviewRepository _userReviewRepository;
		private readonly IUserReviewsCache _userReviewsCache;
		private readonly ICarVersionCacheRepository _carVersionsCache;

		public UserReviewLogic(IUserReviewRepository userReviewRepository, IUserReviewsCache userReviewsCache, ICarVersionCacheRepository carVersionCache)
        {
            _userReviewRepository = userReviewRepository;
            _userReviewsCache = userReviewsCache;
        }
        public string SaveCustomerReview(UserReviewBody reviewBody)
        {
            try
            {
                UserReviewDetails userReviewDetails = new UserReviewDetails();
                userReviewDetails.Id = reviewBody.Id;
                userReviewDetails.Title = SanitizeHTML.ToSafeHtml(reviewBody.Title);
                userReviewDetails.Description = SanitizeHTML.ToSafeHtml(reviewBody.Description);
                if (reviewBody.ReviewQuestions.IsNotNullOrEmpty())
                {
                    foreach (var question in reviewBody.ReviewQuestions)
                    {

                        switch (question.Id)
                        {
                            case (short)EditorialEnum.ReviewQuestions.ExteriorAndStyle:
                                switch (question.RatingId)
                                {
                                    case (short)EditorialEnum.ReviewRating.Poor:
                                        userReviewDetails.ExteriorStyle = (short)EditorialEnum.ReviewRating.Poor;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Good:
                                        userReviewDetails.ExteriorStyle = (short)EditorialEnum.ReviewRating.Good;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.VeryGood:
                                        userReviewDetails.ExteriorStyle = (short)EditorialEnum.ReviewRating.VeryGood;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Fair:
                                        userReviewDetails.ExteriorStyle = (short)EditorialEnum.ReviewRating.Fair;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Excellent:
                                        userReviewDetails.ExteriorStyle = (short)EditorialEnum.ReviewRating.Excellent;
                                        break;
                                }
                                break;
                            case (short)EditorialEnum.ReviewQuestions.ComfortAndSpace:
                                switch (question.RatingId)
                                {
                                    case (short)EditorialEnum.ReviewRating.Poor:
                                        userReviewDetails.Comfort = (short)EditorialEnum.ReviewRating.Poor;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Good:
                                        userReviewDetails.Comfort = (short)EditorialEnum.ReviewRating.Good;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.VeryGood:
                                        userReviewDetails.Comfort = (short)EditorialEnum.ReviewRating.VeryGood;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Fair:
                                        userReviewDetails.Comfort = (short)EditorialEnum.ReviewRating.Fair;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Excellent:
                                        userReviewDetails.Comfort = (short)EditorialEnum.ReviewRating.Excellent;
                                        break;
                                }
                                break;
                            case (short)EditorialEnum.ReviewQuestions.Performance:
                                switch (question.RatingId)
                                {
                                    case (short)EditorialEnum.ReviewRating.Poor:
                                        userReviewDetails.Performance = (short)EditorialEnum.ReviewRating.Poor;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Good:
                                        userReviewDetails.Performance = (short)EditorialEnum.ReviewRating.Good;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.VeryGood:
                                        userReviewDetails.Performance = (short)EditorialEnum.ReviewRating.VeryGood;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Fair:
                                        userReviewDetails.Performance = (short)EditorialEnum.ReviewRating.Fair;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Excellent:
                                        userReviewDetails.Performance = (short)EditorialEnum.ReviewRating.Excellent;
                                        break;
                                }
                                break;
                            case (short)EditorialEnum.ReviewQuestions.FuelEconomy:
                                switch (question.RatingId)
                                {
                                    case (short)EditorialEnum.ReviewRating.Poor:
                                        userReviewDetails.FuelEconomy = (short)EditorialEnum.ReviewRating.Poor;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Good:
                                        userReviewDetails.FuelEconomy = (short)EditorialEnum.ReviewRating.Good;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.VeryGood:
                                        userReviewDetails.FuelEconomy = (short)EditorialEnum.ReviewRating.VeryGood;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Fair:
                                        userReviewDetails.FuelEconomy = (short)EditorialEnum.ReviewRating.Fair;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Excellent:
                                        userReviewDetails.FuelEconomy = (short)EditorialEnum.ReviewRating.Excellent;
                                        break;
                                }
                                break;
                            case (short)EditorialEnum.ReviewQuestions.Value:
                                switch (question.RatingId)
                                {
                                    case (short)EditorialEnum.ReviewRating.Poor:
                                        userReviewDetails.ValueForMoney = (short)EditorialEnum.ReviewRating.Poor;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Good:
                                        userReviewDetails.ValueForMoney = (short)EditorialEnum.ReviewRating.Good;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.VeryGood:
                                        userReviewDetails.ValueForMoney = (short)EditorialEnum.ReviewRating.VeryGood;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Fair:
                                        userReviewDetails.ValueForMoney = (short)EditorialEnum.ReviewRating.Fair;
                                        break;
                                    case (short)EditorialEnum.ReviewRating.Excellent:
                                        userReviewDetails.ValueForMoney = (short)EditorialEnum.ReviewRating.Excellent;
                                        break;
                                }
                                break;
                        }
                    }
                }
                string customerName = _userReviewRepository.SaveUserReview(userReviewDetails);
                _userReviewRepository.SendUserReviewEmail(reviewBody.Id, UserReviewStatus.ReviewSubmmited);
                return customerName;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }
		public UserReviewPageDetails GetWriteReviewPageDetails(int reviewId)
		{
			try
			{
				UserReviewDetail userReviewDetail = _userReviewsCache.GetUserReviewDetailById(reviewId, CMSAppId.Carwale);
				if (userReviewDetail != null)
				{
					UserReviewPageDetails reviewDetails = WriteReviewObject.GetWriteReviewPageDetails(userReviewDetail);
					return reviewDetails;
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return null;
		}

        public UserReveiwLandingPageDetails GetLandingPageDetails(Platform platformId)
        {
            UserReveiwLandingPageDetails details = new UserReveiwLandingPageDetails();
            
            if (platformId == Platform.CarwaleDesktop)
            {
                details.PrimaryBanner = ConfigurationManager.AppSettings["userReviewDesktopPrimaryBanner"] ?? string.Empty;
                details.SecondaryBanner =  ConfigurationManager.AppSettings["userReviewDesktopSecondaryBanner"] ?? string.Empty;
            }
            else
            {
                details.PrimaryBanner = ConfigurationManager.AppSettings["userReviewMobilePrimaryBanner"] ?? string.Empty;
                details.SecondaryBanner =  ConfigurationManager.AppSettings["userReviewMobileSecondaryBanner"] ?? string.Empty;
            }
            details.WriteReview = new SectionDetails { Title = "Stand a chance to win Amazon vouchers in three steps:" };
            details.ReadReview = new SectionDetails { Title = "Want to read reviews? Just search the car below" };
            details.Filters = new Filters
            {
                DefaultFilter = UserReviewLandingObject.AllFilters[0],
                AllFilters = UserReviewLandingObject.AllFilters
            };
            details.TermsAndCondition = new SectionDetails { Title = "Terms & Conditions", Description = UserReviewLandingObject.TermsAndConditions((int)platformId) };
            details.HowToWin = new SectionDetails { Title = "How to win?", Description = UserReviewLandingObject.howToWin };
            return details;
        }
        public int GetVersion(string cookie, int modelId)
        {
            string[] modelVesionPairs = cookie.Split('&');
            foreach (var modelVersion in modelVesionPairs)
            {
                string[] modelVersionPair = modelVersion.Split('~');
                if (modelVersionPair[0].Equals(modelId.ToString()))
                {
                    return Int32.Parse(modelVersionPair[1]);
                }
            }
            return -1;
        }
	}
}
