
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewPageModel
    {
        private readonly IUserReviews _userReviews = null;
        public WriteReviewPageModel(IUserReviews userReviews)
        {
            _userReviews = userReviews;
        }

        public WriteReviewPageVM GetData()
        {
            WriteReviewPageVM objPage = null;
            try
            {
                objPage = new WriteReviewPageVM();

                /* UserReviewQuestion objUser = new UserReviewQuestion();
                objUser.DisplayType = UserReviewQuestionDisplayType.Star;
                objUser.Id = 506;
                objUser.Heading = "Are you sure";
                objUser.Description = "Hello";

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

                UserReviewQuestion objUser1 = new UserReviewQuestion();
                objUser1.DisplayType = UserReviewQuestionDisplayType.Text;
                objUser1.Id = 304;
                objUser1.Heading = "Are you sureeeeee";
                objUser1.Description = "Hell";

                IList<UserReviewRating> obj1 = new List<UserReviewRating>();


                UserReviewRating objReview11 = new UserReviewRating();
                objReview11.Id = 1;
                objReview11.Text = "Failq";
                objReview11.Value = "1";

                UserReviewRating objReview21 = new UserReviewRating();
                objReview21.Id = 2;
                objReview21.Text = "Failureq";
                objReview21.Value = "2";

                UserReviewRating objReview31 = new UserReviewRating();
                objReview31.Id = 3;
                objReview31.Text = "Successq";
                objReview31.Value = "3";

                UserReviewRating objReview41 = new UserReviewRating();
                objReview41.Id = 4;
                objReview41.Text = "Successfulq";
                objReview41.Value = "4";

                UserReviewRating objReview51 = new UserReviewRating();
                objReview51.Id = 5;
                objReview51.Text = "Successssssssq";
                objReview51.Value = "5";


                obj1.Add(objReview11);
                obj1.Add(objReview21);
                obj1.Add(objReview31);
                obj1.Add(objReview41);
                obj1.Add(objReview51);

                objUser1.Rating = obj1;


                ReviewQuestionsDto DTO1 = Convert(objUser1);

                IList<ReviewQuestionsDto> reviewQuestions = new List<ReviewQuestionsDto>();

                reviewQuestions.Add(DTO);
                reviewQuestions.Add(DTO1);

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(reviewQuestions);


                objPage.JsonQuestionList = str; */

                //var objUserReviews = _userReviews.GetUserReviewsData();
                //var objReviewQuestions = objUserReviews.Questions.Where(x => x.Type == UserReviewQuestionType.Rating);
                //foreach (var question in objReviewQuestions)
                //{
                //    question.Rating = objUserReviews.Ratings.Where(x => x.QuestionId == question.Id);
                //}

                //objPage.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objReviewQuestions);

                GetUserRatings(objPage);

            }
            catch (Exception ex)
            {

            }
            return objPage;
        }

        private void GetUserRatings(WriteReviewPageVM objUserVM)
        {
            UserReviewsData objUserReviewData = _userReviews.GetUserReviewsData();
            if (objUserReviewData != null)
            {
                if (objUserReviewData.OverallRating != null)
                {
                    //objUserVM.Rating = objUserReviewData.OverallRating;
                }

                if (objUserReviewData.Questions != null)
                {
                    UserReviewsInputEntity filter = new UserReviewsInputEntity()
                    {
                        Type = UserReviewQuestionType.Review,
                        PriceRangeId = 3
                    };
                    var objQuestions = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                    if (objQuestions != null)
                    {
                        objUserVM.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                    }
                }
            }
        }

    }
}