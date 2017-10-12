﻿using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Models;
using Bikewale.Models.UserReviews;
using Bikewale.Utility;
using Bikewale.Utility.StringExtention;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

using Bikewale.Models.BikeModels;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {

        private readonly IUserReviews _userReviews = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModel = null;
        private readonly IUserReviewsRepository _userReviewsRepo = null;
        private readonly IUserReviewsCache _userReviewsCacheRepo = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IBikeMakesCacheRepository<int> _makesRepository;
        private readonly IUserReviewsCache _userReviewCache = null;

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Constructor to resolve dependencies    
        /// Modified by: Vivek Singh Tomar on 12th Aug 2017
        /// Summary: Added IUserReviewsCache to fetch list of winners of user reviews contest
        /// </summary>
        /// <param name="objArticles"></param>
        /// <param name="cityCache"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviewsCacheRepo"></param>
        /// <param name="userReviews"></param>
        /// <param name="objModel"></param>
        /// <param name="userReviewsRepo"></param>
        /// <param name="userReviewsSearch"></param>
        /// <param name="makesRepository"></param>
        /// <param name="userReviewCache"></param>
        public UserReviewController(ICMSCacheContent objArticles, ICityCacheRepository cityCache, IBikeInfo bikeInfo,
            IUserReviewsCache userReviewsCacheRepo, IUserReviews userReviews, IBikeMaskingCacheRepository<BikeModelEntity, int> objModel,
                IUserReviewsRepository userReviewsRepo, IUserReviewsSearch userReviewsSearch, IBikeMakesCacheRepository<int> makesRepository, IUserReviewsCache userReviewCache)
        {

            _userReviews = userReviews;
            _userReviewsRepo = userReviewsRepo;
            _objModel = objModel;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _userReviewsCacheRepo = userReviewsCacheRepo;
            _userReviewsSearch = userReviewsSearch;
            _objArticles = objArticles;
            _makesRepository = makesRepository;
            _userReviewCache = userReviewCache;
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 7 June 2017
        /// summary    : User review list page for desktop
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("{makeMasking}-bikes/{modelMasking}/reviews/")]
        public ActionResult ListReviews(string makeMasking, string modelMasking, uint? pageNo)
        {
            UserReviewListingPage objData = new UserReviewListingPage(makeMasking, modelMasking, _objModel, _userReviewsCacheRepo, _userReviewsSearch, _objArticles, _userReviewsSearch);
            if (objData.Status.Equals(StatusCodes.ContentFound))
            {
                objData.PageNumber = pageNo;
                objData.ExpertReviewsWidgetCount = 3;
                objData.SimilarBikeReviewWidgetCount = 9;
                UserReviewListingVM objVM = objData.GetData();
                if (objData.Status.Equals(StatusCodes.ContentNotFound))
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (objData.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(objData.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }



        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Action method for mobile user reviews section
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [Route("m/{makeMasking}-bikes/{modelMasking}/reviews/")]
        public ActionResult ListReviews_Mobile(string makeMasking, string modelMasking, uint? pageNo)
        {
            UserReviewListingPage objData = new UserReviewListingPage(makeMasking, modelMasking, _objModel, _userReviewsCacheRepo, _userReviewsSearch, _objArticles, _userReviewsSearch);
            if (objData.Status.Equals(StatusCodes.ContentFound))
            {
                objData.IsMobile = true;
                objData.PageNumber = pageNo;
                objData.ExpertReviewsWidgetCount = 9;
                objData.SimilarBikeReviewWidgetCount = 9;
                UserReviewListingVM objVM = objData.GetData();
                if (objData.Status.Equals(StatusCodes.ContentNotFound))
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else
                {
                    return View(objVM);
                }
            }
            else if (objData.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(objData.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 7 June 2017
        /// summary    : User review details page for desktop
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("user-reviews/details/{reviewId}")]
        public ActionResult ReviewDetails(uint reviewId, string makeMaskingName, string modelMaskingName)
        {
            UserReviewDetailsPage objUserReviewDetails = new UserReviewDetailsPage(reviewId, _userReviewsCacheRepo, _bikeInfo, _cityCache, _objArticles, _objModel, makeMaskingName, modelMaskingName, _userReviewsSearch);

            objUserReviewDetails.TabsCount = 3;
            objUserReviewDetails.ExpertReviewsWidgetCount = 3;
            objUserReviewDetails.SimilarBikeReviewWidgetCount = 9;
            UserReviewDetailsVM objPage = objUserReviewDetails.GetData();

            if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Description.Length > 0 && objPage.ReviewId > 0)
                return View(objPage);
            else
                return Redirect("/pageNotFound.aspx");

        }

        /// <summary>
        /// Created by sajal gupta on  10-05-2017
        /// description : to load user review detail mobile page
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [Route("m/user-reviews/details/{reviewId}")]
        public ActionResult ReviewDetails_Mobile(uint reviewId, string makeMaskingName, string modelMaskingName)
        {
            UserReviewDetailsPage objUserReviewDetails = new UserReviewDetailsPage(reviewId, _userReviewsCacheRepo, _bikeInfo, _cityCache, _objArticles, _objModel, makeMaskingName, modelMaskingName, _userReviewsSearch);

            objUserReviewDetails.IsMobile = true;
            objUserReviewDetails.TabsCount = 3;
            objUserReviewDetails.ExpertReviewsWidgetCount = 3;
            objUserReviewDetails.SimilarBikeReviewWidgetCount = 9;
            UserReviewDetailsVM objPage = objUserReviewDetails.GetData();

            if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Description.Length > 0 && objPage.ReviewId > 0)
                return View(objPage);
            else
                return Redirect("/pageNotFound.aspx");

        }

        /// <summary>
        /// Created by Subodh Jain on 10-04-2017
        /// Description : This action will fetch rate bike page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("user-reviews/rate-bike/{modelId}/")]
        [Filters.DeviceDetection()]
        public ActionResult RateBike(uint modelId, string q, uint? selectedRating)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _userReviews, _objModel, q, _userReviewsRepo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();

            if (objUserReview.status == Entities.StatusCodes.ContentFound)
            {
                objUserReview.PlatFormId = 1;
                UserReviewVM = objUserReview.GetData();
                if (UserReviewVM != null && UserReviewVM.objModelEntity != null)
                    return View(UserReviewVM);
                else
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
            else
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

        }

        /// <summary>
        /// Created by Subodh Jain on 10-04-2017
        /// Description : This action will fetch rate bike page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/user-reviews/rate-bike/{modelId}/")]
        public ActionResult RateBike_Mobile(uint modelId, string q, uint? selectedRating)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _userReviews, _objModel, q, _userReviewsRepo);
            UserReviewRatingVM UserReviewVM = null;

            if (objUserReview.status == Entities.StatusCodes.ContentFound)
            {
                objUserReview.IsMobile = true;
                objUserReview.PlatFormId = 2;
                UserReviewVM = objUserReview.GetData();
            }
            else
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

            if (UserReviewVM != null && UserReviewVM.objModelEntity != null)
                return View(UserReviewVM);
            else
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

        }

        /// <summary>
        /// Created by Subodh Jain on 10-04-2017
        /// Description : This action will submit rating
        /// Modified by : Aditi Srivastava on 29 May 2017
        /// Summary     : Added sourceId parameter
        /// Modified by :Snehal Dange on 7th September 2017
        /// Summary : Added InputRatingSaveEntity
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpPost, Route("user-reviews/ratings/save/"), ValidateAntiForgeryToken]
        public ActionResult SubmitRating(InputRatingSaveEntity objInputRating)
        {
            UserReviewRatingObject objRating = null;
            objInputRating.UserName = StringHelper.ToProperCase(objInputRating.UserName);
            objRating = _userReviews.SaveUserRatings(objInputRating);


            string strQueryString = string.Empty;
            if (objRating != null)
                strQueryString = string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}&priceRangeId={5}&userName={6}&emailId={7}&isFake={8}&returnUrl={9}&sourceid={10}&contestsrc={11}", objRating.ReviewId, objInputRating.MakeId, objInputRating.ModelId, objInputRating.OverAllrating, objRating.CustomerId, objInputRating.PriceRangeId, objInputRating.UserName, objInputRating.EmailId, objRating.IsFake, objInputRating.ReturnUrl, objInputRating.SourceId, objInputRating.ContestSrc);

            string strEncoded = Utils.Utils.EncryptTripleDES(strQueryString);
            if (objRating != null && !objRating.IsFake)
            {
                if (objInputRating.IsDesktop.HasValue && objInputRating.IsDesktop.Value)
                    return Redirect("/write-a-review/?q=" + strEncoded);
                else
                    return Redirect("/m/write-a-review/?q=" + strEncoded);
            }
            else
            {
                return Redirect(string.Format("/rate-your-bike/{0}/?q={1}", objInputRating.ModelId, strEncoded));
            }


        }

        /// <summary>
        /// Created by Sajal Gupta on 10-04-2017
        /// Description : This action will fetch write review page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("user-reviews/write-review/")]
        public ActionResult WriteReview(string q)
        {
            WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, q);

            if (!string.IsNullOrEmpty(q))
            {
                objPage.IsDesktop = true;
                var objData = objPage.GetData();

                if (objData != null && objData.ReviewId > 0 && objData.CustomerId > 0 && objPage.Status == Entities.StatusCodes.ContentFound)
                {
                    return View(objData);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }



        /// <summary>
        /// Created by Sajal Gupta on 10-04-2017
        /// Description : This action will fetch write review page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile(string q)
        {
            WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, q);

            if (!string.IsNullOrEmpty(q))
            {
                objPage.IsMobile = true;
                var objData = objPage.GetData();

                if (objData != null && objData.ReviewId > 0 && objData.CustomerId > 0 && objPage.Status == Entities.StatusCodes.ContentFound)
                {
                    return View(objData);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Action method to save user reviews
        /// Modified by Sajal Gupta on 13-07-2017
        /// Descrfiption : Added mileage field.
        /// Modified By :   Vishnu Teja Yalakuntla on 09 Sep 2017
        /// Description :   Decoded encodedId
        /// </summary>
        /// <param name="reviewDescription"></param>
        /// <param name="reviewTitle"></param>
        /// <param name="reviewQuestion"></param>
        /// <param name="reviewTips"></param>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        /// 
        [ValidateInput(false)]
        [HttpPost, Route("user-reviews/save/"), ValidateAntiForgeryToken]
        public ActionResult SaveReview(ReviewSubmitData objReviewData, bool? fromParametersRatingScreen)
        {
            try
            {
                WriteReviewPageSubmitResponse objResponse = null;

                uint decodedReviewId;
                ulong decodedCustomerId;

                string decodedString = Utils.Utils.DecryptTripleDES(objReviewData.EncodedId);
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedString);

                uint.TryParse(queryCollection["reviewid"], out decodedReviewId);
                ulong.TryParse(queryCollection["customerid"], out decodedCustomerId);

                objReviewData.ReviewId = decodedReviewId;
                objReviewData.CustomerId = decodedCustomerId;

                objResponse = _userReviews.SaveUserReviews(objReviewData);

                if (objResponse.IsSuccess)
                {
                    if (objReviewData.IsDesktop.HasValue && objReviewData.IsDesktop.Value && fromParametersRatingScreen.HasValue && !fromParametersRatingScreen.Value)
                        return Redirect(string.Format("/parameter-ratings/?q={0}", objReviewData.EncodedString));
                    else if (objReviewData.IsDesktop.HasValue && objReviewData.IsDesktop.Value)
                        return Redirect(string.Format("/user-reviews/review-summary/{0}/?q={1}", objReviewData.ReviewId, objReviewData.EncodedString));
                    else
                        return Redirect(string.Format("/m/user-reviews/review-summary/{0}/?q={1}", objReviewData.ReviewId, objReviewData.EncodedString));
                }
                else
                {
                    WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, objReviewData.EncodedString);
                    var objData = objPage.GetData();
                    objData.SubmitResponse = objResponse;
                    return View("WriteReview_Mobile", objData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controllers.UserReviewController.SaveReview()");
            }
            return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
        }


        /// <summary>
        /// Created by sajal gupta on 31-08-2017
        /// description : Controller for rate other details p[age
        /// </summary>
        /// <param name="encodedString"></param>
        /// <returns></returns>
        [Route("parameter-ratings/")]
        public ActionResult RateOtherDetails(string q)
        {
            try
            {
                UserReviewOtherDetailsPage objRateOtherDetails = new UserReviewOtherDetailsPage(_userReviews, q);
                UserReviewsOtherDetailsPageVM objPageData = null;
                if (objRateOtherDetails.Status == StatusCodes.ContentFound)
                {
                    objPageData = objRateOtherDetails.GetData();
                    return View(objPageData);
                }
                else
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "RateOtherDetails()");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Subodh Jain on 10-04-2017
        /// Description : To fetch review summary page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("user-reviews/review-summary/{reviewid}/")]
        public ActionResult ReviewSummary(uint reviewid, string q)
        {
            if (reviewid > 0)
            {
                UserReviewSummaryPage objData = new UserReviewSummaryPage(_userReviews, reviewid, q);
                objData.IsDesktop = true;
                if (objData.status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/pageNotFound.aspx");
                }
                UserReviewSummaryVM objVM = objData.GetData();
                if (objData.status == Entities.StatusCodes.ContentFound)
                    return View(objVM);
                else
                    return Redirect("/pageNotFound.aspx");
            }
            else
            {
                return Redirect("/pageNotFound.aspx");
            }
        }     		

    /// <summary>
    /// Created by : Aditi Srivastava on 19 Apr 2017
    /// Summary    : To fetch review summary page
    /// </summary>
    [Route("m/user-reviews/review-summary/{reviewid}/")]
        public ActionResult ReviewSummary_Mobile(uint reviewid, string q)
        {
            if (reviewid > 0)
            {
                UserReviewSummaryPage objData = new UserReviewSummaryPage(_userReviews, reviewid, q);
                objData.IsMobile = true;
                if (objData.status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/pageNotFound.aspx");
                }
                UserReviewSummaryVM objVM = objData.GetData();
                if (objData.status == Entities.StatusCodes.ContentFound)
                    return View(objVM);
                else
                    return Redirect("/pageNotFound.aspx");
            }
            else
            {
                return Redirect("/pageNotFound.aspx");
            }
        }


        /// <summary>
        /// Summary: Controller to fetch contest data
        /// Created by: Sangram Nandkhile on 05 June 2017
        /// Modified by: Vivek Singh Tomar on 12th Aug 2017
        /// Summary: Added _userReviewCache to get winners of user review contest
        /// Modified By :Snehal Dange on 25th Sep 2017
        /// Descrption : Added MakeId and ModelId 
        /// </summary>
        [Route("m/user-reviews/contest/")]
        public ActionResult ReviewContest_Mobile(string q)
        {
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(q));
            uint makeId = 0, modelId = 0;
            int? csrc ;
            string makeName, modelName, makeMaskingName, modelMaskingName;

            csrc = Convert.ToInt16(queryCollection["csrc"]);
            uint.TryParse(queryCollection["makeId"], out makeId);
            uint.TryParse(queryCollection["modelId"], out modelId);
            makeName = queryCollection["makeName"];
            modelName = queryCollection["modelName"];
            makeMaskingName=queryCollection["makeMaskingName"];
            modelMaskingName=queryCollection["modelMaskingName"];
            
            WriteReviewContest objData = new WriteReviewContest(true, _makesRepository, _userReviewCache, makeId , modelId ,makeName, modelName, makeMaskingName, modelMaskingName);
            objData.IsMobile = true;
            objData.csrc = csrc.HasValue ? csrc.Value : 0;
            WriteReviewContestVM objVM = objData.GetData();
            return View(objVM);
        }

        [Filters.DeviceDetection()]
        [Route("user-reviews/contest/")]
        public ActionResult ReviewContest(string q)
        {
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(q));
            uint makeId = 0, modelId = 0;
            int? csrc;
            string makeName, modelName, makeMaskingName, modelMaskingName;

            csrc = Convert.ToInt16(queryCollection["csrc"]);
            uint.TryParse(queryCollection["makeId"], out makeId);
            uint.TryParse(queryCollection["modelId"], out modelId);
            makeName = queryCollection["makeName"];
            modelName = queryCollection["modelName"];
            makeMaskingName = queryCollection["makeMaskingName"];
            modelMaskingName = queryCollection["modelMaskingName"];

            WriteReviewContest objData = new WriteReviewContest(false, _makesRepository, _userReviewCache, makeId , modelId , makeName, modelName, makeMaskingName, modelMaskingName);
            objData.csrc = csrc.HasValue ? csrc.Value : 0;
            WriteReviewContestVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: Review
        [Route("review/")]
        public ActionResult Index()
        {
            UserReviewLandingPage obj = new UserReviewLandingPage(_userReviewsCacheRepo, _makesRepository);
            return View(obj.GetData());
        }

        // GET: Review
        [Route("m/review/")]
        public ActionResult Index_Mobile()
        {
            UserReviewLandingPage obj = new UserReviewLandingPage(_userReviewsCacheRepo, _makesRepository);
            return View(obj.GetData()); 
        }

    }
}