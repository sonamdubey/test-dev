using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>

        public UserReviewController(IUserReviews userReviews, IBikeMaskingCacheRepository<BikeModelEntity, int> objModel, IUserReviewsRepository userReviewsRepo)
        {

            _userReviews = userReviews;
            _userReviewsRepo = userReviewsRepo;
            _objModel = objModel;


        }

        [Route("user-reviews/rate-bike/{modelId}/")]
        public ActionResult RateBike(uint modelId, uint? pagesourceid, string reviewId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, pagesourceid, _userReviews, _objModel, reviewId, _userReviewsRepo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (objUserReview != null)
            {
                if (objUserReview.status == Entities.StatusCodes.ContentFound)
                {
                    objUserReview.IsDesktop = true;
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


        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}/")]
        public ActionResult RateBike_Mobile(uint modelId, uint? pagesourceid, string reviewId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, pagesourceid, _userReviews, _objModel, reviewId, _userReviewsRepo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (objUserReview != null)
            {
                if (objUserReview.status == Entities.StatusCodes.ContentFound)
                    UserReviewVM = objUserReview.GetData();
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

        [HttpPost, Route("user-reviews/ratings/save/"), ValidateAntiForgeryToken]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint priceRangeId, uint reviewId, uint pagesourceId, bool? isDesktop)
        {


            UserReviewRatingObject objRating = null;
            objRating = _userReviews.SaveUserRatings(overAllrating, ratingQuestionAns, userName, emailId, makeId, modelId, pagesourceId, reviewId);
            string strQueryString = string.Empty;
            if (objRating != null)
                strQueryString = string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}&priceRangeId={5}&userName={6}&emailId={7}&pagesourceid={8}&isFake={9}", objRating.ReviewId, makeId, modelId, overAllrating, objRating.CustomerId, priceRangeId, userName, emailId, pagesourceId, objRating.IsFake);

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
                return Redirect(string.Format("/rate-your-bike/{0}/?reviewId={1}", modelId, strEncoded));
            }


        }

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


        [Route("user-reviews/review-summary/{reviewid}/")]
        public ActionResult ReviewSummary(uint reviewid, string q)
        {
            if (reviewid > 0)
            {
                UserReviewSummaryPage objData = new UserReviewSummaryPage(_userReviews, reviewid, q);
                if (objData != null && objData.status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/m/pageNotFound.aspx");
                }
                UserReviewSummaryVM objVM = objData.GetData();
                if (objData.status == Entities.StatusCodes.ContentFound)
                    return View(objVM);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
            else
            {
                return Redirect("/m/pageNotFound.aspx");
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
                    return Redirect("/m/pageNotFound.aspx");
                }
                UserReviewSummaryVM objVM = objData.GetData();
                if (objData.status == Entities.StatusCodes.ContentFound)
                    return View(objVM);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
            else
            {
                return Redirect("/m/pageNotFound.aspx");
            }
        }
    }
}