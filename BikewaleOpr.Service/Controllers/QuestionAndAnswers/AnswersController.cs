using Bikewale.Notifications;
using BikewaleOpr.DTO.QuestionsAnswers;
using BikewaleOpr.Entity.QuestionsAndAnswers;
using BikewaleOpr.Interface.QuestionsAnswers;
using BikewaleOpr.Service.AutoMappers;
using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.QuestionAndAnswers
{
    public class AnswersController : ApiController
    {
        private readonly IAnswersBAL _answers;
        public AnswersController(IAnswersBAL answers)
        {
            _answers = answers;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Controller method for getting all the answers related to a particular question.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/questions/{questionId}/answers/")]
        public IHttpActionResult GetAnswers([FromUri]string questionId)
        {
            AnswersEntity answers = null;
            try
            {
                answers = new AnswersEntity();
                answers.Answers = _answers.GetAnswers(questionId);
                if (answers == null || answers.Answers.Count() == 0)
                {
                    return Ok(string.Format("No answer for the requested QuestionID i.e. '{0}' can be found", questionId));
                }
                AnswersDTO answersDTO = QuestionAnswerMapper.Convert(answers);
                return Ok(answersDTO);
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.Service.Controllers.QuestionAndAnswers.AnswersController.GetAnswers()__QuestionID : {0}", questionId));
                return InternalServerError();
            }
        }
    }
}