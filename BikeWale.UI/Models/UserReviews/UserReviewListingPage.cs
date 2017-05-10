
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using System;
using System.Web;
namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// 
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

        private uint _modelId = 0;

        public UserReviewListingPage(string makeMasking, string modelMasking, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IUserReviewsCache userReviewCache, IUserReviewsSearch objUserReviewSearch, ICMSCacheContent objArticles)
        {
            _objModelMaskingCache = objModelMaskingCache;
            _objUserReviewCache = userReviewCache;
            _objUserReviewSearch = objUserReviewSearch;
            _objArticles = objArticles;
            ParseQueryString(makeMasking, modelMasking);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal UserReviewListingVM GetData()
        {
            UserReviewListingVM objData = new UserReviewListingVM();
            if (_modelId > 0)
            {
                objData.ModelId = _modelId;
                objData.RatingReviewData = _objUserReviewCache.GetBikeRatingsReviewsInfo(_modelId);
                if (objData.RatingReviewData != null && objData.RatingsInfo != null && objData.RatingsInfo.Make != null && objData.RatingsInfo.Model != null)
                {
                    objData.BikeName = string.Format("{0} {1}", objData.RatingsInfo.Make.MakeName, objData.RatingsInfo.Model.ModelName);
                    objData.PageUrl = string.Format("/{0}-bikes/{1}/reviews/", objData.RatingsInfo.Make.MaskingName, objData.RatingsInfo.Model.MaskingName);
                }

                BindWidgets(objData);

                BindPageMetas(objData);
            }
            return objData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        private void BindWidgets(UserReviewListingVM objData)
        {
            InputFilters filters = new InputFilters()
            {
                Model = _modelId.ToString(),
                SO = 2,
                PN = (int)(PageNumber.HasValue ? PageNumber.Value : 1),
                PS = 8,
                Reviews = true
            };


            if (objData.RatingsInfo != null)
            {
                var objUserReviews = new UserReviewsSearchWidget(_modelId, filters, _objUserReviewCache);


                if (objData.ReviewsInfo != null)
                {
                    objData.ReviewsInfo.Make = objData.RatingsInfo.Make;
                    objData.ReviewsInfo.Model = objData.RatingsInfo.Model;
                }

                objUserReviews.ReviewsInfo = objData.ReviewsInfo;
                objData.UserReviews = objUserReviews.GetData();

                objData.ExpertReviews = new RecentExpertReviews(9, (uint)objData.ReviewsInfo.Make.MakeId, (uint)objData.ReviewsInfo.Model.ModelId, objData.ReviewsInfo.Make.MakeName, objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.ModelName, objData.ReviewsInfo.Model.MaskingName, _objArticles, string.Format("Expert Reviews on {0} {1}", objData.ReviewsInfo.Make.MakeName, objData.ReviewsInfo.Model.ModelName)).GetData();

                objData.SimilarBikeReviewWidget = _objModelMaskingCache.GetSimilarBikesUserReviews((uint)objData.ReviewsInfo.Model.ModelId, 9);

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
                            Status = StatusCodes.RedirectPermanent;
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objResponse.MaskingName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.ParseQueryString()");
                Status = StatusCodes.ContentNotFound;
            }
        }

        public void BindPageMetas(UserReviewListingVM objPage)
        {
            try
            {
                if (objPage != null && objPage.PageMetaTags != null && objPage.ReviewsInfo != null)
                {
                    objPage.PageMetaTags.Title = string.Format("{0} {1} Reviews | {1} User Reviews – BikeWale", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);
                    objPage.PageMetaTags.Description = string.Format("Read {0} {1} reviews from genuine buyers and know the pros and cons of {1}. Also, find reviews on {1} from BikeWale experts.", objPage.ReviewsInfo.Make.MakeName, objPage.ReviewsInfo.Model.ModelName);
                    objPage.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/{1}/reviews/", objPage.ReviewsInfo.Make.MaskingName, objPage.ReviewsInfo.Model.MaskingName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewListingPage.BindPageMetas()");
            }
        }

    }
}