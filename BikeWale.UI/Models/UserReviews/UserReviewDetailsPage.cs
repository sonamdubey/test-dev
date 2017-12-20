
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
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
        private readonly IBikeModels<BikeModelEntity, int> _models;

        private readonly uint _reviewId;
        private uint _modelId;
        private BikeSeriesEntityBase Series;

        public uint TabsCount { get; set; }
        public uint ExpertReviewsWidgetCount { get; set; }
        public uint SimilarBikeReviewWidgetCount { get; set; }
        public bool IsMobile { get; internal set; }

        public UserReviewDetailsPage(uint reviewId, IUserReviewsCache userReviewsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, ICMSCacheContent objArticles, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache, string makeMaskingName, string modelMaskingName, IUserReviewsSearch userReviewsSearch, IBikeModels<BikeModelEntity, int> models)
        {
            _reviewId = reviewId;
            _userReviewsCache = userReviewsCache;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _objArticles = objArticles;
            _bikeModelsCache = bikeModelsCache;
            _userReviewsSearch = userReviewsSearch;
            _models = models;
        }

        public UserReviewDetailsVM GetData()
        {
            UserReviewDetailsVM objPage = null;
            try
            {

                UpdateViewCount();

                objPage = new UserReviewDetailsVM();
                objPage.SimilarBikesWidget = new UserReviewSimilarBikesWidgetVM();
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
                ErrorClass.LogError(ex, string.Format("UserReviewDetailsPage.GetData() - ReviewId :{0}", _reviewId));
            }
            return objPage;
        }

        public void BindQuestions(UserReviewDetailsVM objPage)
        {
            try
            {
                objPage.RatingQuestions = new Collection<UserReviewQuestion>();
                objPage.ReviewQuestions = new Collection<UserReviewQuestion>();

                if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Questions != null)
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
                ErrorClass.LogError(ex, string.Format("UserReviewDetailsPage.BindQuestions() - ReviewId :{0}", _reviewId));
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
                ErrorClass.LogError(ex, string.Format("UserReviewDetailsPage.UpdateViewCount() - ReviewId :{0}", _reviewId));
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

                    objPage.PageMetaTags.Title = string.Format("{0} | User Review on {1} {2} - BikeWale", objPage.UserReviewDetailsObj.Title, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName);
                    objPage.Page_H1 = objPage.UserReviewDetailsObj.Title.Truncate(45);

                    objPage.AdTags.TargetedMakes = objPage.UserReviewDetailsObj.Make.MakeName;
                    objPage.AdTags.TargetedModel = objPage.UserReviewDetailsObj.Model.ModelName;
                    objPage.PageMetaTags.Description = string.Format("Read review by {0} on {1} {2}. {0} says {3}. View detailed review on BikeWale.", objPage.UserReviewDetailsObj.CustomerName, objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName, objPage.UserReviewDetailsObj.Title);
                    objPage.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/{2}/", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);
                    objPage.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/reviews/{2}/", objPage.UserReviewDetailsObj.Make.MaskingName, objPage.UserReviewDetailsObj.Model.MaskingName, _reviewId);


                    Series = _models.GetSeriesByModelId(_modelId);
                    SetBreadcrumList(objPage);
                    SetPageJSONLDSchema(objPage);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewDetailsPage.BindPageMetas() - ReviewId :{0}", _reviewId));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by Sajal Gupt on 10-11-2017       
        /// desccription : Changed breadcrumbs
        /// </summary>
        private void SetBreadcrumList(UserReviewDetailsVM objPage)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl, scooterUrl, seriesUrl;
            bikeUrl = scooterUrl = seriesUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";

            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

            if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Make != null)
            {
                bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, objPage.UserReviewDetailsObj.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", objPage.UserReviewDetailsObj.Make.MakeName)));
            }

            if (objPage.GenericBikeWidgetData != null && objPage.GenericBikeWidgetData.BikeInfo != null &&
                objPage.GenericBikeWidgetData.BikeInfo.BodyStyleId.Equals((sbyte)EnumBikeBodyStyles.Scooter) && !(objPage.UserReviewDetailsObj.Make.IsScooterOnly))
            {
                if (IsMobile)
                {
                    scooterUrl += "m/";
                }
                scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objPage.UserReviewDetailsObj.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objPage.UserReviewDetailsObj.Make.MakeName)));
            }

            if (Series != null && Series.IsSeriesPageUrl)
            {
                seriesUrl = string.Format("{0}{1}/", bikeUrl, Series.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, Series.SeriesName));
            }

            if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Model != null && objPage.UserReviewDetailsObj.Make != null)
            {
                bikeUrl = string.Format("{0}{1}/", bikeUrl, objPage.UserReviewDetailsObj.Model.MaskingName);
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", objPage.UserReviewDetailsObj.Make.MakeName, objPage.UserReviewDetailsObj.Model.ModelName)));
            }

            if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Model != null)
            {
                bikeUrl += "reviews/";
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, objPage.UserReviewDetailsObj.Model.ModelName + " Reviews"));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPage.Page_H1));

            objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(UserReviewDetailsVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        public void BindUserReviewSWidget(UserReviewDetailsVM objPage)
        {
            try
            {
                // Set default category to be loaded here
                FilterBy activeReviewCateory = FilterBy.MostRecent;

                ReviewDataCombinedFilter objFilter = new ReviewDataCombinedFilter()
                {
                    InputFilter = new Entities.UserReviews.Search.InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = 1,
                        PS = IsMobile ? 8 : 10,
                        Reviews = true,
                        SkipReviewId = _reviewId
                    },
                    ReviewFilter = new ReviewFilter()
                    {
                        RatingQuestion = !IsMobile,
                        ReviewQuestion = false,
                        SantizeHtml = true,
                        SanitizedReviewLength = (uint)(IsMobile ? 150 : 270),
                        BasicDetails = true
                    }
                };

                var objUserReviews = new UserReviewsSearchWidget(_modelId, objFilter, _userReviewsCache, _userReviewsSearch);
                objUserReviews.IsDesktop = !IsMobile;


                objUserReviews.ActiveReviewCateory = activeReviewCateory;

                objPage.UserReviews = objUserReviews.GetData();

                if (objPage.UserReviews != null)
                {
                    if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Model != null)
                        objPage.UserReviews.WidgetHeading = string.Format("More reviews on {0}", objPage.UserReviewDetailsObj.Model.ModelName);

                    objPage.UserReviews.IsPagerNeeded = false;

                    if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.OverallRating != null && objPage.UserReviews != null && objPage.UserReviews.ReviewsInfo != null && objPage.UserReviews.ReviewsInfo.MostRecentReviews > 1)
                    {
                        objPage.UserReviews.ReviewsInfo.MostRecentReviews = objPage.UserReviews.ReviewsInfo.MostRecentReviews - 1;
                        objPage.UserReviews.ReviewsInfo.MostHelpfulReviews = objPage.UserReviews.ReviewsInfo.MostHelpfulReviews - 1;

                        if (objPage.UserReviewDetailsObj.OverallRating.Value <= 3)
                        {
                            objPage.UserReviews.ReviewsInfo.NegativeReviews = objPage.UserReviews.ReviewsInfo.NegativeReviews - 1;
                        }
                        else
                        {
                            objPage.UserReviews.ReviewsInfo.PostiveReviews = objPage.UserReviews.ReviewsInfo.PostiveReviews - 1;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewDetailsPage.BindUserReviewSWidget() - ReviewId :{0}", _reviewId));
            }
        }
    }
}