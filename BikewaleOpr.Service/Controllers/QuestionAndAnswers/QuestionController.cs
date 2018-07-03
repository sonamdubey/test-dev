using Bikewale.Notifications;
using BikewaleOpr.DTO.QuestionsAnswers;
using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.Question;
using BikewaleOpr.Entity.QuestionsAndAnswers.Question;
using BikewaleOpr.Interface.QnA;
using BikewaleOpr.Service.AutoMappers;
using QuestionsAnswers.BAL;
using QuestionsAnswers.Entities;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.QuestionAndAnswers
{
    public class QuestionController : ApiController
    {
        private readonly IQuestionsBAL _questions = null;

        public QuestionController(IQuestionsBAL questions)
        {
            _questions = questions;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 11 June 2018
        /// Description : Controller for Moderating the Question with Id = {QuestionId}
        /// </summary>
        /// <param name="QuestionId">Id of the question to be moderated</param>
        /// <param name="ModeratedBy">Id of the user who is moderating the question</param>
        /// <param name="Action">Action to be performed on the question e.g. `Approve`, `Reject` etc.</param>
        /// <param name="sendEmail">Boolean denoting whether the question moderation mail should be sent to the user or not.</param>
        /// <returns></returns>
        [HttpPost, Route("api/questions/{questionId}/moderate/{actionType}/")]
        public IHttpActionResult ModerateQuestion([FromBody]ModerateQuestionDTO moderateQuestion, [FromUri]string questionId, [FromUri]QuestionsAnswers.Entities.EnumModerationActionType actionType, bool sendEmail = true)
        {
            if (moderateQuestion == null)
            {
                return BadRequest();
            }
            ModerateQuestionEntity moderateQuestionEntity = QuestionAnswerMapper.Convert(moderateQuestion);
            try
            {
                QuestionModerationResponse questionModerationResponse = _questions.ModerateQuestion(questionId, moderateQuestionEntity, actionType, sendEmail);
                if (questionModerationResponse != null)
                {
                    return Ok(questionModerationResponse);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("QuestionController.ModerateQuestion(QuestionId:{0}, ModeratedBy:{1}, Action:{2})", questionId, moderateQuestionEntity.ModeratedBy, actionType));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 14 June 2018
        /// Description : Controller for updating the tags for Question with given question id.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/questions/{questionId}/updatetags/")]
        public IHttpActionResult UpdateQuestionTags([FromUri]string questionId, [FromBody] UpdateQuestionTagsDTO updateQuestionTags)
        {
            UpdateQuestionTagsEntity updateQuestionTagsEntity = QuestionAnswerMapper.Convert(updateQuestionTags);
            try
            {
                UpdateQuestionTagsResponse response = _questions.UpdateQuestionTags(questionId, updateQuestionTagsEntity);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("QuestionController.UpdateQuestionTags(QuestionId:{0}, ModeratedBy:{1}, Action:{2})", questionId));
                return InternalServerError();
            }
        }


        /// <summary>
        /// Created by :Snehal Dange on 19th June 2018
        /// Description : Action method for saving answer in opr
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/questions/{questionId}/answer/")]
        public IHttpActionResult SaveQuestionAnswer([FromUri]string questionId, [FromBody] AnswerDTO answerDTO, ushort platformId, ushort sourceId)
        {
            bool isSaveSuccessful = false;
            try
            {
                if (answerDTO != null)
                {
                    if (string.IsNullOrEmpty(answerDTO.QuestionId))
                    {
                        answerDTO.QuestionId = questionId;
                    }
                    Answer answerEntity = QuestionAnswerMapper.Convert(answerDTO);
                    isSaveSuccessful = _questions.SaveQuestionAnswer(answerEntity, platformId, sourceId);
                    if (isSaveSuccessful)
                    {
                        return Ok("Answer successfully saved");
                    }
                    else
                    {
                        return Ok("Error : Answer Not saved");
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("QuestionController.SaveQuestionAnswer(QuestionId:{0})", answerDTO.QuestionId));
                return InternalServerError();
            }
        }
    }
}
