
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
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
        private readonly IUserReviewsSearch _userReviewsSearch = null;

        private uint _reviewId;
        private uint _modelId;
        private string _makeMaskingName;
        private string _modelMaskingName;

        public uint TabsCount { get; set; }
        public uint ExpertReviewsWidgetCount { get; set; }
        public uint SimilarBikeReviewWidgetCount { get; set; }
        public bool IsDesktop { get; set; }

        public UserReviewDetailsPage(uint reviewId, IUserReviewsCache userReviewsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, ICMSCacheContent objArticles, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache, string makeMaskingName, string modelMaskingName, IUserReviewsSearch userReviewsSearch)
        {
            _reviewId = reviewId;
            _userReviewsCache = userReviewsCache;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _objArticles = objArticles;
            _bikeModelsCache = bikeModelsCache;
            _makeMaskingName = makeMaskingName;
            _modelMaskingName = modelMaskingName;
            _userReviewsSearch = userReviewsSearch;
        }

        public UserReviewDetailsVM GetData()
        {
            UserReviewDetailsVM objPage = null;
            try
            {
                objPage.SimilarBikesWidget = new UserReviewSimilarBikesWidgetVM();

                UpdateViewCount();

                objPage = new UserReviewDetailsVM();
                objPage.UserReviewDetailsObj = _userReviewsCache.GetUserReviewSummaryWithRating(_reviewId);

                if (objPage.UserReviewDetailsObj != null)
                {
                    _modelId = (uint)objPage.UserReviewDetailsObj.Model.ModelId;
                }

                objPage.ReviewId = _reviewId;

                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                BikeInfoWidget genericBikeModel = new BikeInfoWidget(_bikeInfo, _cityCache, _modelId, currentCityArea.CityId, TabsCount, BikeInfoTabType.UserReview);
                objPage.GenericBikeWidgetData = genericBikeModel.GetData();
                objPage.GenericBikeWidgetData.IsSmallSlug = false;

                if (objPage.UserReviewDetailsObj != null)
                {
                    objPage.ExpertReviews = new RecentExpertReviews(ExpertReviewsWidgetCount, (uint)objPage.UserReviewDetailsObj.Make.MakeId, _modelId, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.ModelName, objPage.UserReviewDetailsObj.Model.MaskingName, _objArticles, string.Format("Expert Reviews on {0}", objPage.UserReviewDetailsObj.Model.ModelName)).GetData();
                }

                objPage.SimilarBikesWidget.SimilarBikes = _bikeModelsCache.GetSimilarBikesUserReviews(_modelId, currentCityArea.CityId, SimilarBikeReviewWidgetCount);
                objPage.SimilarBikesWidget.GlobalCityName = currentCityArea.City;

                BindQuestions(objPage);

                BindPageMetas(objPage);

                BindUserReviewSWidget(objPage);

                if (objPage.UserReviewDetailsObj != null)
                {
                    if (objPage.UserReviewDetailsObj.Make != null && objPage.UserReviewDetailsObj.Model != null)
                        objPage.PageUrl = string.Format("/{0}-bikes/{1}/reviews/{2}/", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);

                    objPage.ReviewAge = FormatDate.GetTimeSpan(objPage.UserReviewDetailsObj.EntryDate);
                }
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

                if (objPage.UserReviewDetailsObj != null)
                {
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
        /// <summary>
        /// Modified :- Subodh Jain 19 june 2017
        /// summary :- added targetmodel and make
        /// </summary>
        public void BindPageMetas(UserReviewDetailsVM objPage)
        {
            try
            {
                if (objPage != null && objPage.PageMetaTags != null && objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Make != null && objPage.UserReviewDetailsObj.Model != null)
                {
                    objPage.AdTags.TargetedMakes = objPage.UserReviewDetailsObj.Make.MakeName;
                    objPage.AdTags.TargetedModel = objPage.UserReviewDetailsObj.Model.ModelName;
                    objPage.PageMetaTags.Title = string.Format("{0} | User Review on {1} {2} - BikeWale", objPage.UserReviewDetailsObj.Title, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName);
                    objPage.PageMetaTags.Description = string.Format("Read review by {0} on {1} {2}. {0} says {3}. View detailed review on BikeWale.", objPage.UserReviewDetailsObj.CustomerName, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName, objPage.UserReviewDetailsObj.Title);
                    objPage.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/{2}/", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);
                    objPage.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/reviews/{2}/", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.BindPageMetas() - ReviewId :{0}", _reviewId));
            }
        }

        public void BindUserReviewSWidget(UserReviewDetailsVM objPage)
        {
            try
            {
                InputFilters filters = null;
                // Set default category to be loaded here
                FilterBy activeReviewCateory = FilterBy.MostHelpful;
                if (!IsDesktop)
                {
                    filters = new InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = 1,
                        PS = 8,
                        Reviews = true,
                        SkipReviewId = _reviewId
                    };
                }
                else
                {
                    filters = new InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = 1,
                        PS = 10,
                        Reviews = true,
                        SkipReviewId = _reviewId
                    };
                }


                var objUserReviews = new UserReviewsSearchWidget(_modelId, filters, _userReviewsCache, _userReviewsSearch);
                objUserReviews.IsDesktop = IsDesktop;

                if (objUserReviews != null)
                {
                    objUserReviews.ActiveReviewCateory = activeReviewCateory;

                    objPage.UserReviews = objUserReviews.GetData();

                    if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Model != null)
                        objPage.UserReviews.WidgetHeading = string.Format("More reviews on {0}", objPage.UserReviewDetailsObj.Model.ModelName);

                    objPage.UserReviews.IsPagerNeeded = false;

                    if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.OverallRating != null && objPage.UserReviews != null && objPage.UserReviews.ReviewsInfo != null && objPage.UserReviews.ReviewsInfo.MostRecentReviews > 1)
                    {
                        objPage.UserReviews.ReviewsInfo.MostRecentReviews = objPage.UserReviews.ReviewsInfo.MostRecentReviews - 1;
                        objPage.UserReviews.ReviewsInfo.MostHelpfulReviews = objPage.UserReviews.ReviewsInfo.MostHelpfulReviews - 1;

                        if (objPage.UserReviewDetailsObj.OverallRating.Value == 3)
                        {
                            objPage.UserReviews.ReviewsInfo.NeutralReviews = objPage.UserReviews.ReviewsInfo.NeutralReviews - 1;
                        }
                        else if (objPage.UserReviewDetailsObj.OverallRating.Value > 3)
                        {
                            objPage.UserReviews.ReviewsInfo.PostiveReviews = objPage.UserReviews.ReviewsInfo.PostiveReviews - 1;
                        }
                        else
                        {
                            objPage.UserReviews.ReviewsInfo.NegativeReviews = objPage.UserReviews.ReviewsInfo.NegativeReviews - 1;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewDetailsPage.BindUserReviewSWidget() - ReviewId :{0}", _reviewId));
            }
        }
    }
}