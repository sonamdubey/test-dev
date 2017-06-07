using Bikewale.Common;
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
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {

        private readonly IUserReviews _userReviews = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> _objModel = null;
        private readonly IUserReviewsRepository _userReviewsRepo = null;
        private readonly IUserReviewsCache _userReviewsCacheRepo = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly ICMSCacheContent _objArticles = null;       

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Constructor to resolve dependencies
        /// </summary>
        /// <param name="objArticles"></param>
        /// <param name="cityCache"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviewsCacheRepo"></param>
        /// <param name="userReviews"></param>
        /// <param name="objModel"></param>
        /// <param name="userReviewsRepo"></param>
        /// <param name="userReviewsSearch"></param>
        public UserReviewController(ICMSCacheContent objArticles, ICityCacheRepository cityCache, IBikeInfo bikeInfo, IUserReviewsCache userReviewsCacheRepo, IUserReviews userReviews, IBikeMaskingCacheRepository<BikeModelEntity, int> objModel, IUserReviewsRepository userReviewsRepo, IUserReviewsSearch userReviewsSearch)
        {

            _userReviews = userReviews;
            _userReviewsRepo = userReviewsRepo;
            _objModel = objModel;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _userReviewsCacheRepo = userReviewsCacheRepo;
            _userReviewsSearch = userReviewsSearch;
            _objArticles = objArticles;
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
            if (objData != null && objData.Status.Equals(StatusCodes.ContentFound))
            {
                objData.PageNumber = pageNo;
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
        /// Created by sajal gupta on  10-05-2017
        /// description : to load user review detail mobile page
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [Route("m/user-reviews/details/{reviewId}")]
        public ActionResult ReviewDetails_Mobile(uint reviewId, string makeMaskingName, string modelMaskingName)
        {
            UserReviewDetailsPage objUserReviewDetails = new UserReviewDetailsPage(reviewId, _userReviewsCacheRepo, _bikeInfo, _cityCache, _objArticles, _objModel, makeMaskingName, modelMaskingName, _userReviewsSearch);
            if (objUserReviewDetails != null)
            {
                objUserReviewDetails.TabsCount = 3;
                objUserReviewDetails.ExpertReviewsWidgetCount = 3;
                objUserReviewDetails.SimilarBikeReviewWidgetCount = 9;
                UserReviewDetailsVM objPage = objUserReviewDetails.GetData();

                if (objPage.UserReviewDetailsObj != null && objPage.UserReviewDetailsObj.Description.Length > 0 && objPage.ReviewId > 0)
                    return View(objPage);
                else
                    return Redirect("/pageNotFound.aspx");
            }
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
            if (objUserReview != null)
            {
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
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (objUserReview != null)
            {
                if (objUserReview.status == Entities.StatusCodes.ContentFound)
                {
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
            else
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

        }

        /// <summary>
        /// Created by Subodh Jain on 10-04-2017
        /// Description : This action will submit rating
        /// Modified by : Aditi Srivastava on 29 May 2017
        /// Summary     : Added sourceId parameter
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpPost, Route("user-reviews/ratings/save/"), ValidateAntiForgeryToken]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint priceRangeId, uint reviewId, bool? isDesktop, string returnUrl, ushort platformId, ushort? sourceId)
        {


            UserReviewRatingObject objRating = null;

            objRating = _userReviews.SaveUserRatings(overAllrating, ratingQuestionAns, userName, emailId, makeId, modelId, reviewId, returnUrl, platformId, sourceId);


            string strQueryString = string.Empty;


            if (objRating != null)
                strQueryString = string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}&priceRangeId={5}&userName={6}&emailId={7}&isFake={8}&returnUrl={9}&sourceid={10}", objRating.ReviewId, makeId, modelId, overAllrating, objRating.CustomerId, priceRangeId, userName, emailId, objRating.IsFake, returnUrl, sourceId);

            string strEncoded = Utils.Utils.EncryptTripleDES(strQueryString);
            if (objRating != null && !objRating.IsFake)
            {
                if (isDesktop.HasValue && isDesktop.Value)
                    return Redirect("/write-a-review/?q=" + strEncoded);
                else
                    return Redirect("/m/write-a-review/?q=" + strEncoded);
            }
            else
            {
                return Redirect(string.Format("/rate-your-bike/{0}/?q={1}", modelId, strEncoded));
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

            if (objPage != null && !string.IsNullOrEmpty(q))
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

            if (objPage != null && !string.IsNullOrEmpty(q))
            {
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
        public ActionResult SaveReview(string reviewDescription, string reviewTitle, string reviewQuestion, string reviewTips, string encodedId, string emailId, string userName, string makeName, string modelName, uint reviewId, string encodedString, bool? isDesktop)
        {
            WriteReviewPageSubmitResponse objResponse = null;

            objResponse = _userReviews.SaveUserReviews(encodedId, reviewTips.Trim(), reviewDescription, reviewTitle, reviewQuestion, emailId, userName, makeName, modelName);

            if (objResponse.IsSuccess)
            {
                if (isDesktop.HasValue && isDesktop.Value)
                    return Redirect(string.Format("/user-reviews/review-summary/{0}/?q={1}", reviewId, encodedString));
                else
                    return Redirect(string.Format("/m/user-reviews/review-summary/{0}/?q={1}", reviewId, encodedString));
            }
            else
            {
                WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, encodedString);
                var objData = objPage.GetData();
                objData.SubmitResponse = objResponse;
                return View("WriteReview_Mobile", objData);
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
                if (objData != null && objData.status == Entities.StatusCodes.ContentNotFound)
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
                if (objData != null && objData.status == Entities.StatusCodes.ContentNotFound)
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

        [Route("user-reviews/model")]
        public ActionResult ListReviews()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        [Route("user-reviews/details")]
        public ActionResult ReviewDetails()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}