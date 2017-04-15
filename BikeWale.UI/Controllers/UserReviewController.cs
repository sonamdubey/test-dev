using AutoMapper;
using Bikewale.Interfaces.BikeData;
using Bikewale.DTO.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Models;
using System.Collections;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        private readonly IBikeInfo _bikeInfo = null;

        public UserReviewController(IBikeInfo bikeInfo)
        {
            _bikeInfo = bikeInfo;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}")]
        public ActionResult RateBike_Mobile(uint modelId)
        {
            UserReviewRating objUserReview = new UserReviewRating(modelId, _bikeInfo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            UserReviewVM = objUserReview.GetData();
            return View(UserReviewVM);
        }

        //[HttpPost]
        //public ActionResult SubmitRating()
        //{
        //    return false;
        //}


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

            IList<UserReviewrating> obj = new List<UserReviewrating>();


            UserReviewrating objReview1 = new UserReviewrating();
            objReview1.Id = 1;
            objReview1.Text = "Fail";
            objReview1.Value = "1";

            UserReviewrating objReview2 = new UserReviewrating();
            objReview2.Id = 2;
            objReview2.Text = "Failure";
            objReview2.Value = "2";

            UserReviewrating objReview3 = new UserReviewrating();
            objReview3.Id = 3;
            objReview3.Text = "Success";
            objReview3.Value = "3";

            UserReviewrating objReview4 = new UserReviewrating();
            objReview4.Id = 4;
            objReview4.Text = "Successful";
            objReview4.Value = "4";

            UserReviewrating objReview5 = new UserReviewrating();
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
            Mapper.CreateMap<UserReviewrating, UserReviewratingDto>();
            Mapper.CreateMap<UserReviewQuestion, ReviewQuestionsDto>();
            return Mapper.Map<UserReviewQuestion, ReviewQuestionsDto>(objUserReviewQuestion);
        }
    }
}