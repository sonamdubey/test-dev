using AutoMapper;
using Bikewale.DTO.UserReviews;
using Bikewale.Entities.UserReviews;
using Bikewale.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        // GET: UserReview
        [Route("m/user-reviews/rate-bike/")]
        public ActionResult RateBike_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
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

            IList<UserReviewrating> obj = new List<UserReviewrating>();


            UserReviewrating objReview1 = new UserReviewrating();
            objReview1.Id = 1;
            objReview1.Text = "Fail";
            objReview1.Value = "1";

            UserReviewrating objReview2 = new UserReviewrating();
            objReview2.Id = 1;
            objReview2.Text = "Failure";
            objReview2.Value = "1";

            UserReviewrating objReview3 = new UserReviewrating();
            objReview3.Id = 1;
            objReview3.Text = "Success";
            objReview3.Value = "1";


            obj.Add(objReview1);
            obj.Add(objReview2);
            obj.Add(objReview3);

            objUser.Rating = obj;

            string str = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(objUser);

            ReviewQuestionsDto DTO = Convert(objUser);

            return View(m);
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