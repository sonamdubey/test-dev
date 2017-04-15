using AutoMapper;
using Bikewale.DTO.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IUserReviews _userReviews = null;

        public UserReviewController(IBikeInfo bikeInfo, IUserReviews userReviews)
        {
            _bikeInfo = bikeInfo;
            _userReviews = userReviews;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}")]
        public ActionResult RateBike_Mobile(uint modelId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _bikeInfo, _userReviews);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            UserReviewVM = objUserReview.GetData();
            return View(UserReviewVM);
        }

        [HttpPost, Route("user-reviews/ratings/save/")]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId)
        {
            return View();
        }


        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile()
        {
            ModelBase m = new ModelBase();

            UserReviewQuestion objUser = new UserReviewQuestion();
            objUser.DisplayType = UserReviewQuestionDisplayType.Star;
            objUser.Id = 1;
            objUser.Heading = "Are you sure";
            objUser.Description = "Hello";
            objUser.SelectedRatingId = 1;

            IList<UserReviewRating> obj = new List<UserReviewRating>();


            UserReviewRating objReview1 = new UserReviewRating();
            objReview1.Id = 1;
            objReview1.Text = "Fail";
            objReview1.Value = "1";

            UserReviewRating objReview2 = new UserReviewRating();
            objReview2.Id = 2;
            objReview2.Text = "Failure";
            objReview2.Value = "2";

            UserReviewRating objReview3 = new UserReviewRating();
            objReview3.Id = 3;
            objReview3.Text = "Success";
            objReview3.Value = "3";

            UserReviewRating objReview4 = new UserReviewRating();
            objReview4.Id = 4;
            objReview4.Text = "Successful";
            objReview4.Value = "4";

            UserReviewRating objReview5 = new UserReviewRating();
            objReview5.Id = 5;
            objReview5.Text = "Successssssss";
            objReview5.Value = "5";


            obj.Add(objReview1);
            obj.Add(objReview2);
            obj.Add(objReview3);
            obj.Add(objReview4);
            obj.Add(objReview5);

            objUser.Rating = obj;



            ReviewQuestionsDto DTO = Convert(objUser);



            IList<ReviewQuestionsDto> reviewQuestions = new List<ReviewQuestionsDto>();

            reviewQuestions.Add(DTO);

            string str = Newtonsoft.Json.JsonConvert.SerializeObject(reviewQuestions);

            WriteReviewPageVM objPage = new WriteReviewPageVM();

            objPage.JsonQuestionList = str;


            return View(objPage);
        }

        [Route("m/user-reviews/review-summary/")]
        public ActionResult ReviewSummary_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        private ReviewQuestionsDto Convert(UserReviewQuestion objUserReviewQuestion)
        {
            Mapper.CreateMap<UserReviewQuestionDisplayType, UserReviewQuestionDisplayTypeDto>();
            Mapper.CreateMap<UserReviewRating, UserReviewratingDto>();
            Mapper.CreateMap<UserReviewQuestion, ReviewQuestionsDto>();
            return Mapper.Map<UserReviewQuestion, ReviewQuestionsDto>(objUserReviewQuestion);
        }
    }
}