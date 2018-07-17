using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QuestionsAndAnswers;
using BikewaleOpr.Interface.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.QuestionAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 09 July 2018
    /// Description : Controller to generate URLs for questions and answers
    /// </summary>
    public class UrlGeneratorController : ApiController
    {
        private readonly IQuestionsBAL _questions;
        public UrlGeneratorController(IQuestionsBAL questions)
        {
            _questions = questions;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 09 July 2018
        /// Description : Function to generate URL for answering the question.
        /// </summary>
        /// <param name="answerEmailInfo"></param>
        /// <returns></returns>
        [HttpPost, Route("api/urlgenerator/cheetahmail/")]
        public IHttpActionResult CheetahMailUrl([FromBody]IEnumerable<AnswerEmailInfo> answerEmailInfo)
        {
            try
            {
                if (answerEmailInfo == null || !answerEmailInfo.Any())
                {
                    return BadRequest();
                }
                foreach (var req in answerEmailInfo)
                {
                    string queryString = Utils.Utils.EncryptTripleDES(string.Format(@"userEmail={0}&userName={1}&questionId={2}", req.UserEmail, req.UserName, req.QuestionId));
                    req.Url = String.Format("{0}/questions-and-answers/answer/?q={1}", BWConfiguration.Instance.BwHostUrl, queryString);
                }
                return Ok(answerEmailInfo);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.QuestionAndAnswers.UrlGeneratorController.CheetahMailUrl()");
                return Ok();
            }
        }
    }
}
