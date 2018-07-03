using Bikewale.DTO.QuestionAndAnswers;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.QuestionAndAnswers;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;

namespace Bikewale.Service.Controllers.QuestionAndAnswers
{
    public class QuestionsController : CompressionApiController
    {
        private readonly IQuestions _questions = null;

        public QuestionsController(IQuestions questions)
        {
            _questions = questions;
        }

        /// <summary>
        /// Created By : Deepak Israni on 12 June 2018
        /// Description: API to enable user to ask questions.
        /// </summary>
        /// <param name="objQuestionInput"></param>
        /// <returns></returns>
        [HttpPost, Route("api/questions/")]
        public IHttpActionResult AskQuestion(QuestionDTO objQuestionInput)
        {
            try
            {
                if (objQuestionInput != null && objQuestionInput.PlatformId > 0 && objQuestionInput.SourceId > 0)
                {
                    Guid? questionId = _questions.SaveQuestion(QuestionMapper.Convert(objQuestionInput), objQuestionInput.PlatformId, objQuestionInput.SourceId);
                    if (questionId != null)
                    {
                        return Ok(questionId);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.QuestionAndAnswers.AskQuestion");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 22 June 2018
        /// Description : API to get Questions for a particular `modelId` with given `pageNo` and `pageSize`(no of elements)
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet, Route("api/models/{modelId}/questions/")]
        public IHttpActionResult GetQuestionsByModelId([FromUri]uint modelId, ushort pageNo = 1, ushort pageSize = 10)
        {
            QuestionsDTO questionsDTO = null;
            try
            {
                if (modelId == 0)
                {
                    return BadRequest();
                }
                Questions questions = _questions.GetQuestionsByModelId(modelId, pageNo, pageSize);
                if (questions == null || questions.QuestionList == null || questions.QuestionList.Count() == 0)
                {
                    return NotFound();
                }

                questionsDTO = QuestionMapper.Convert(questions);

                return Ok(questionsDTO);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.QuestionAndAnswers.GetQuestionsByModelId() | modelId={0}, pageNo={1}, pageSize={2}", modelId, pageNo, pageSize));
                return InternalServerError();
            }
        }


    }
}
