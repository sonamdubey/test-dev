
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 05-05-201\7
    /// Description : Model for user review details page
    /// </summary>
    public class UserReviewDetailsPage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeModelsCache = null;

        private uint _reviewId;

        public uint TabsCount { get; set; }
        public uint ExpertReviewsWidgetCount { get; set; }
        public uint SimilarBikeReviewWidgetCount { get; set; }

        public UserReviewDetailsPage(uint reviewId, IUserReviewsCache userReviewsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, ICMSCacheContent objArticles, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache)
        {
            _reviewId = reviewId;
            _userReviewsCache = userReviewsCache;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _objArticles = objArticles;
            _bikeModelsCache = bikeModelsCache;
        }

        public UserReviewDetailsVM GetData()
        {
            UserReviewDetailsVM objPage = null;
            try
            {
                UpdateViewCount();

                objPage = new UserReviewDetailsVM();
                objPage.UserReviewDetailsObj = _userReviewsCache.GetUserReviewSummaryWithRating(_reviewId);

                objPage.ReviewId = _reviewId;

                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                BikeInfoWidget genericBikeModel = new BikeInfoWidget(_bikeInfo, _cityCache, (uint)objPage.UserReviewDetailsObj.Model.ModelId, currentCityArea.CityId, TabsCount, BikeInfoTabType.UserReview);
                objPage.GenericBikeWidgetData = genericBikeModel.GetData();
                objPage.GenericBikeWidgetData.IsSmallSlug = false;

                objPage.ExpertReviews = new RecentExpertReviews(ExpertReviewsWidgetCount, (uint)objPage.UserReviewDetailsObj.Make.MakeId, (uint)objPage.UserReviewDetailsObj.Model.ModelId, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.ModelName, objPage.UserReviewDetailsObj.Model.MaskingName, _objArticles, string.Format("Expert Reviews on {0} {1}", objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName)).GetData();

                objPage.SimilarBikeReviewWidget = _bikeModelsCache.GetSimilarBikesUserReviews((uint)objPage.UserReviewDetailsObj.Model.ModelId, SimilarBikeReviewWidgetCount);

                BindQuestions(objPage);

                BindPageMetas(objPage);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.GetData() - ReviewId :{0}", _reviewId));
            }
            return objPage;
        }

        public void BindQuestions(UserReviewDetailsVM objPage)
        {
            try
            {
                objPage.RatingQuestions = new Collection<UserReviewQuestion>();
                objPage.ReviewQuestions = new Collection<UserReviewQuestion>();

                foreach (UserReviewQuestion ques in objPage.UserReviewDetailsObj.Questions)
                {
                    if (ques.Type == UserReviewQuestionType.Rating)
                    {
                        if (ques.SelectedRatingId != 0)
                            objPage.RatingQuestions.Add(ques);
                    }
                    else
                        objPage.ReviewQuestions.Add(ques);
                }

                objPage.RatingQuestionCount = (uint)objPage.RatingQuestions.Count;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.BindQuestions() - ReviewId :{0}", _reviewId));
            }
        }

        public void UpdateViewCount()
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("par_reviewId", _reviewId.ToString());
                SyncBWData.PushToQueue("updateUserReviewViews", DataBaseName.BW, nvc);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.UpdateViewCount() - ReviewId :{0}", _reviewId));
            }
        }

        public void BindPageMetas(UserReviewDetailsVM objPage)
        {
            try
            {
                if (objPage != null && objPage.PageMetaTags != null && objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Make != null && objPage.UserReviewDetailsObj.Model != null)
                {
                    objPage.PageMetaTags.Title = string.Format("{0} | User Review on {1} {2} - BikeWale", objPage.UserReviewDetailsObj.Title, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName);
                    objPage.PageMetaTags.Description = string.Format("Read review by {0} on {1} {2}. {0} says {3}. View detailed review on BikeWale.", objPage.UserReviewDetailsObj.CustomerName, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName, objPage.UserReviewDetailsObj.Title);
                    objPage.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/{2}", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.BindPageMetas() - ReviewId :{0}", _reviewId));
            }
        }
    }
}