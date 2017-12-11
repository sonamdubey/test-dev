﻿
using System;
using System.Collections.Generic;
using System.Web;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Utility;
namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : Model for user reviews listing page
    /// </summary>
    public class UserReviewListingPage
    {
        public string RedirectUrl { get; set; }
        public StatusCodes Status { get; set; }
        public uint? PageNumber { get; set; }

        private readonly IUserReviewsSearch _objUserReviewSearch;
        private readonly IUserReviewsCache _objUserReviewCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IBikeModels<BikeModelEntity, int> _models;
        private uint _modelId = 0;
        private uint _pageSize, _totalResults;
        public uint ExpertReviewsWidgetCount { get; set; }
        public uint SimilarBikeReviewWidgetCount { get; set; }
        public bool IsMobile { get; internal set; }
        private BikeSeriesEntityBase Series;

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Constructor to resolve dependencies
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="objModelMaskingCache"></param>
        /// <param name="userReviewCache"></param>
        /// <param name="objUserReviewSearch"></param>
        /// <param name="objArticles"></param>
        public UserReviewListingPage(string makeMasking, string modelMasking, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IUserReviewsCache userReviewCache, IUserReviewsSearch objUserReviewSearch, ICMSCacheContent objArticles, IUserReviewsSearch userReviewsSearch, IBikeModels<BikeModelEntity, int> models)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objUserReviewCache = userReviewCache;
            _objUserReviewSearch = objUserReviewSearch;
            _objArticles = objArticles;
            _userReviewsSearch = userReviewsSearch;
            _models = models;
            ParseQueryString(makeMasking, modelMasking);
        }


        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Function to get list review page data
        /// </summary>
        /// <returns></returns>
        internal UserReviewListingVM GetData()
        {
            UserReviewListingVM objData = new UserReviewListingVM();
            try
            {
                if (_modelId > 0)
                {
                    objData.ModelId = _modelId;
                    objData.RatingReviewData = _objUserReviewCache.GetBikeRatingsReviewsInfo(_modelId);
                    if (objData.RatingReviewData == null)
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    else if (objData.RatingReviewData != null && objData.RatingsInfo != null && objData.RatingsInfo.Make != null && objData.RatingsInfo.Model != null)
                    {
                        objData.BikeName = string.Format("{0} {1}", objData.RatingsInfo.Make.MakeName, objData.RatingsInfo.Model.ModelName);
                        objData.PageUrl = string.Format("/{0}-bikes/{1}/reviews/", objData.RatingsInfo.Make.MaskingName, objData.RatingsInfo.Model.MaskingName);
                        objData.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/", objData.RatingsInfo.Make.MaskingName, objData.RatingsInfo.Model.MaskingName);
                        objData.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/reviews/", objData.RatingsInfo.Make.MaskingName, objData.RatingsInfo.Model.MaskingName);
                    }

                    BindWidgets(objData);
                    BindPageMetas(objData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewListingPage.GetData()");
                Status = StatusCodes.ContentNotFound;
            }
            return objData;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Function to bind widgets
        /// </summary>
        /// <param name="objData"></param>
        private void BindWidgets(UserReviewListingVM objData)
        {
            try
            {
                objData.SimilarBikesWidget = new UserReviewSimilarBikesWidgetVM();

                FilterBy activeReviewCateory = FilterBy.MostHelpful;
                _pageSize = (uint)(IsMobile ? 8 : 10);
                ReviewDataCombinedFilter objFilter = new ReviewDataCombinedFilter()
                {
                    InputFilter = new Entities.UserReviews.Search.InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = (int)(PageNumber.HasValue ? PageNumber.Value : 1),
                        PS = (int)_pageSize,
                        Reviews = true
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

                if (objData.RatingsInfo != null)
                {
                    var objUserReviews = new UserReviewsSearchWidget(_modelId, objFilter, _objUserReviewCache, _userReviewsSearch);

                    objUserReviews.ActiveReviewCateory = activeReviewCateory;

                    if (objData.ReviewsInfo != null)
                    {
                        objData.ReviewsInfo.Make = objData.RatingsInfo.Make;
                        objData.ReviewsInfo.Model = objData.RatingsInfo.Model;
                        objData.ReviewsInfo.IsDiscontinued = objData.RatingsInfo.IsDiscontinued;
                        objUserReviews.ReviewsInfo = objData.ReviewsInfo;
                    }

                    objData.UserReviews = objUserReviews.GetData();

                    if (objData.UserReviews != null)
                    {
                        objData.UserReviews.WidgetHeading = string.Format("Reviews on {0}", objData.RatingsInfo.Model.ModelName);
                        if (objData.UserReviews.Pager != null)
                        {
                            _totalResults = (uint)objData.UserReviews.Pager.TotalResults;
                        }
                    }



                    objData.ExpertReviews = new RecentExpertReviews(ExpertReviewsWidgetCount, (uint)objData.ReviewsInfo.Make.MakeId, (uint)objData.ReviewsInfo.Model.ModelId, objData.ReviewsInfo.Make.MakeName, objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.ModelName, objData.ReviewsInfo.Model.MaskingName, _objArticles, string.Format("Expert Reviews on {0}", objData.ReviewsInfo.Model.ModelName)).GetData();

                    GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                    objData.SimilarBikesWidget.SimilarBikes = _objModelMaskingCache.GetSimilarBikesUserReviews((uint)objData.ReviewsInfo.Model.ModelId, currentCityArea.CityId, SimilarBikeReviewWidgetCount);
                    objData.SimilarBikesWidget.GlobalCityName = currentCityArea.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewListingPage.BindWidgets()");
            }
        }

        private void ParseQueryString(string makeMasking, string modelMasking)
        {
            ModelMaskingResponse objResponse = null;
            Status = StatusCodes.ContentNotFound;
            try
            {
                if (!string.IsNullOrEmpty(modelMasking))
                {
                    objResponse = _objModelMaskingCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMasking, modelMasking));

                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objResponse.MaskingName);
                            Status = StatusCodes.RedirectPermanent;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewListingPage.ParseQueryString()");
            }
        }
        /// <summary>
        /// Modified :- Subodh Jain 19 june 2017
        /// summary :- added targetmodel and make
        /// </summary>
        /// <param name="objPage"></param>
        public void BindPageMetas(UserReviewListingVM objPage)
        {
            try
            {
                if (objPage != null && objPage.PageMetaTags != null && objPage.ReviewsInfo != null)
                {
                    objPage.AdTags.TargetedMakes = objPage.ReviewsInfo.Make.MakeName;
                    objPage.AdTags.TargetedModel = objPage.ReviewsInfo.Model.ModelName;
                    objPage.PageMetaTags.Title = string.Format("{0} {1} Reviews | Reviews from Users & Experts", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);
                    objPage.PageMetaTags.Description = string.Format("Read {0} {1} reviews from genuine buyers and know the pros and cons of {1}. Also, find reviews on {1} from BikeWale experts.", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);

                    uint _totalPagesCount = (uint)(_totalResults / _pageSize);

                    if ((_totalResults % _pageSize) > 0)
                        _totalPagesCount += 1;

                    if (PageNumber > 1)
                    {
                        objPage.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", PageNumber, _totalPagesCount, objPage.PageMetaTags.Description);
                        objPage.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", PageNumber, _totalPagesCount, objPage.PageMetaTags.Title);
                    }

                    objPage.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/", objPage.ReviewsInfo.Make.MaskingName, objPage.ReviewsInfo.Model.MaskingName);

                    uint curPageNo = PageNumber.HasValue ? PageNumber.Value : 1;


                    if (curPageNo > 1)
                    {
                        objPage.PageMetaTags.PreviousPageUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/page/{2}/", objPage.ReviewsInfo.Make.MaskingName, objPage.ReviewsInfo.Model.MaskingName, curPageNo - 1);
                    }
                    if ((curPageNo * _pageSize) < _totalResults)
                    {
                        objPage.PageMetaTags.NextPageUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/page/{2}/", objPage.ReviewsInfo.Make.MaskingName, objPage.ReviewsInfo.Model.MaskingName, curPageNo + 1);
                    }

                    Series = _models.GetSeriesByModelId(_modelId);

                    SetBreadcrumList(objPage);
                    SetPageJSONLDSchema(objPage);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewListingPage.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(UserReviewListingVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(UserReviewListingVM objPage)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            string scooterUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            string seriesUrl = string.Empty;
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}reviews/", bikeUrl), "Reviews"));

            if (objPage.RatingsInfo != null && objPage.RatingsInfo.Make != null)
            {
                bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, objPage.RatingsInfo.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", objPage.RatingsInfo.Make.MakeName)));
            }
            if (objPage.RatingReviewData != null && objPage.RatingReviewData.RatingDetails != null &&
                objPage.RatingReviewData.RatingDetails.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !(objPage.RatingsInfo.Make.IsScooterOnly))
            {
                if (IsMobile)
                {
                    scooterUrl += "m/";
                }
                scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objPage.RatingsInfo.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objPage.RatingsInfo.Make.MakeName)));
            }

            if (Series != null && Series.IsSeriesPageUrl)
            {
                seriesUrl = string.Format("{0}{1}/", bikeUrl, Series.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, Series.SeriesName));
            }

            if (objPage.RatingsInfo != null && objPage.RatingsInfo.Model != null)
            {
                bikeUrl = string.Format("{0}{1}/", bikeUrl, objPage.RatingsInfo.Model.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", objPage.RatingsInfo.Make.MakeName, objPage.RatingsInfo.Model.ModelName)));

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPage.RatingsInfo.Model.ModelName + " Reviews"));

            }

            objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

    }
}