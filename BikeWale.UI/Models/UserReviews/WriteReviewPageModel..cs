
using AutoMapper;
using Bikewale.DTO.UserReviews;
using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewPageModel
    {
        public WriteReviewPageVM GetData()
        {
            WriteReviewPageVM objPage = null;
            try
            {
                objPage = new WriteReviewPageVM();

                UserReviewQuestion objUser = new UserReviewQuestion();
                objUser.DisplayType = UserReviewQuestionDisplayType.Star;
                objUser.Id = 506;
                objUser.Heading = "Are you sure";
                objUser.Description = "Hello";

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

                UserReviewQuestion objUser1 = new UserReviewQuestion();
                objUser1.DisplayType = UserReviewQuestionDisplayType.Text;
                objUser1.Id = 304;
                objUser1.Heading = "Are you sureeeeee";
                objUser1.Description = "Hell";

                IList<UserReviewrating> obj1 = new List<UserReviewrating>();


                UserReviewrating objReview11 = new UserReviewrating();
                objReview11.Id = 1;
                objReview11.Text = "Failq";
                objReview11.Value = "1";

                UserReviewrating objReview21 = new UserReviewrating();
                objReview21.Id = 2;
                objReview21.Text = "Failureq";
                objReview21.Value = "2";

                UserReviewrating objReview31 = new UserReviewrating();
                objReview31.Id = 3;
                objReview31.Text = "Successq";
                objReview31.Value = "3";

                UserReviewrating objReview41 = new UserReviewrating();
                objReview41.Id = 4;
                objReview41.Text = "Successfulq";
                objReview41.Value = "4";

                UserReviewrating objReview51 = new UserReviewrating();
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


                objPage.JsonQuestionList = str;



            }
            catch (Exception ex)
            {

            }
            return objPage;
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