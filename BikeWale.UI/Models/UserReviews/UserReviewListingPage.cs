
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Models;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;
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
        public bool IsDesktop { get; set; }

        private readonly IUserReviewsSearch _objUserReviewSearch;
        private readonly IUserReviewsCache _objUserReviewCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private uint _modelId = 0;
        private uint _pageSize, _totalResults;

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
        public UserReviewListingPage(string makeMasking, string modelMasking, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IUserReviewsCache userReviewCache, IUserReviewsSearch objUserReviewSearch, ICMSCacheContent objArticles, IUserReviewsSearch userReviewsSearch)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objUserReviewCache = userReviewCache;
            _objUserReviewSearch = objUserReviewSearch;
            _objArticles = objArticles;
            _userReviewsSearch = userReviewsSearch;
            ParseQueryString(makeMasking, modelMasking);
        }
        public uint ExpertReviewsWidgetCount { get; set; }
        public uint SimilarBikeReviewWidgetCount { get; set; }
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
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.GetData()");
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

                InputFilters filters = null;

                // Set default category to be loaded here
                FilterBy activeReviewCateory = FilterBy.MostRecent;
                if (!IsDesktop)
                {
                    filters = new InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = (int)(PageNumber.HasValue ? PageNumber.Value : 1),
                        PS = 8,
                        Reviews = true
                    };
                    _pageSize = 8;
                }
                else
                {
                    filters = new InputFilters()
                    {
                        Model = _modelId.ToString(),
                        SO = (ushort)activeReviewCateory,
                        PN = (int)(PageNumber.HasValue ? PageNumber.Value : 1),
                        PS = 10,
                        Reviews = true
                    };
                    _pageSize = 10;
                }


                if (objData.RatingsInfo != null)
                {
                    var objUserReviews = new UserReviewsSearchWidget(_modelId, filters, _objUserReviewCache, _userReviewsSearch);
                    objUserReviews.ActiveReviewCateory = activeReviewCateory;
                    if (objUserReviews != null)
                    {
                        if (objData.ReviewsInfo != null)
                        {
                            objData.ReviewsInfo.Make = objData.RatingsInfo.Make;
                            objData.ReviewsInfo.Model = objData.RatingsInfo.Model;
                            objData.ReviewsInfo.IsDiscontinued = objData.RatingsInfo.IsDiscontinued;
                            objUserReviews.ReviewsInfo = objData.ReviewsInfo;
                        }
                        objData.UserReviews = objUserReviews.GetData();

                        objData.UserReviews.WidgetHeading = string.Format("Reviews on {0}", objData.RatingsInfo.Model.ModelName);

                        if (objData != null && objData.UserReviews != null && objData.UserReviews.Pager != null)
                            _totalResults = (uint)objData.UserReviews.Pager.TotalResults;
                    }
                    objData.ExpertReviews = new RecentExpertReviews(ExpertReviewsWidgetCount, (uint)objData.ReviewsInfo.Make.MakeId, (uint)objData.ReviewsInfo.Model.ModelId, objData.ReviewsInfo.Make.MakeName, objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.ModelName, objData.ReviewsInfo.Model.MaskingName, _objArticles, string.Format("Expert Reviews on {0}", objData.ReviewsInfo.Model.ModelName)).GetData();

                    GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                    objData.SimilarBikesWidget.SimilarBikes = _objModelMaskingCache.GetSimilarBikesUserReviews((uint)objData.ReviewsInfo.Model.ModelId, currentCityArea.CityId, SimilarBikeReviewWidgetCount);
                    objData.SimilarBikesWidget.GlobalCityName = currentCityArea.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.BindWidgets()");
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
                    objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMasking);

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
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.ParseQueryString()");
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
                    objPage.PageMetaTags.Title = string.Format("{0} {1} Reviews | {1} User Reviews – BikeWale", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);
                    objPage.PageMetaTags.Description = string.Format("Read {0} {1} reviews from genuine buyers and know the pros and cons of {1}. Also, find reviews on {1} from BikeWale experts.", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);
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

                    List<BreadCrumb> BreadCrumbs = new List<BreadCrumb>();

                    BreadCrumbs.Add(new BreadCrumb
                    {
                        ListUrl = "/",
                        Name = "Home"
                    });

                    if (objPage.RatingsInfo != null && objPage.RatingsInfo.Make != null)
                    {
                        BreadCrumbs.Add(new BreadCrumb
                        {
                            ListUrl = string.Format("/{0}-bikes/", objPage.RatingsInfo.Make.MaskingName) ,
                            Name = objPage.RatingsInfo.Make.MakeName + " Bikes"
                        });
                    }

                    if (objPage.RatingsInfo != null && objPage.RatingsInfo.Make != null && objPage.RatingsInfo.Model != null)
                    {
                        BreadCrumbs.Add(new BreadCrumb
                        {
                            ListUrl = UrlFormatter.BikePageUrl(objPage.RatingsInfo.Make.MaskingName, objPage.RatingsInfo.Model.MaskingName),
                            Name = objPage.BikeName
                        });
                    }

                    objPage.BreadCrumbsList.Breadcrumbs = BreadCrumbs;
                    objPage.BreadCrumbsList.PageName = "User Reviews";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.BindPageMetas()");
            }
        }

    }
}