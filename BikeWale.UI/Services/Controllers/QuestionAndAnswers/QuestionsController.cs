using Bikewale.DTO.QuestionAndAnswers;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.QuestionAndAnswers;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using BikewaleElasticDTO = Bikewale.DTO.QuestionAndAnswers.ElasticSearch;
using BikeWaleElasticEntities = Bikewale.Entities.QuestionAndAnswers.ElasticSearch;

namespace Bikewale.Service.Controllers.QuestionAndAnswers
{
    public class QuestionsController : CompressionApiController
    {
        private readonly IQuestions _questions = null;
        private const byte TEN_QUESTIONS = 10;

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
                    string clientIp = Bikewale.Utility.CurrentUser.GetClientIP();
                    Guid? questionId = _questions.SaveQuestion(QuestionMapper.Convert(objQuestionInput), objQuestionInput.PlatformId, objQuestionInput.SourceId, clientIp);
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

        /// <summary>
        /// Created by  : Kumar Swapnil on 14 September 2018
        /// Description : API to get Unanswered Questions for a particular `modelId` with given `userEmail`
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="returnUrl"></param>
        /// <param name="platformId"></param>
        /// <param name="sourceId"></param>
        /// <param name="questionLimit"></param>
        /// <returns></returns>
        [HttpGet, Route("api/models/{modelId}/unanswered-questions/")]
        public IHttpActionResult GetUnansweredQuestions([FromUri]uint modelId, string userName, string userEmail, string returnUrl, ushort platformId, ushort sourceId, int questionLimit = TEN_QUESTIONS)
        {
            IEnumerable<QuestionURLDTO> responseQuestions = null;
            IEnumerable<Question> questionList = _questions.GetRemainingUnansweredQuestions(modelId, questionLimit, userEmail);
            ICollection<QuestionUrl> questionUrls = new Collection<QuestionUrl>();

            if (questionList != null)
            {
                try
                {
                    foreach (var question in questionList)
                    {
                        string queryString = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format(@"userEmail={0}&userName={1}&questionId={2}&modelId={3}", userEmail, userName, question.Id, modelId));
                        questionUrls.Add(new QuestionUrl
                        {
                            QuestionData = question,
                            AnsweringUrl = String.Format("{0}/{1}questions-and-answers/answer/?q={2}&source={3}&returnUrl={4}", BWConfiguration.Instance.BwHostUrl, (platformId == 2 ? "m/" : ""), queryString, sourceId, returnUrl)
                        });

                    }
                    responseQuestions = QuestionMapper.Convert<IEnumerable<QuestionUrl>, IEnumerable<QuestionURLDTO>>(questionUrls);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.QuestionAndAnswers.GetUnansweredQuestions() | modelId={0} | userEmail = {1} | userName {2}", modelId, userEmail, userName));
                    return InternalServerError();
                }

            }
            return Ok(responseQuestions);

        }
        /// <summary>
        /// Created By : Snehal Dange on 17 Oct 2018
        /// Description : Api to get relevant questions on search
        /// Modified By : Monika Korrapati on 17 Oct 2018
        /// Description : Mapped to DTO and null checks
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet, Route("api/qna/search/")]
        public IHttpActionResult GetQuestionList([FromUri] uint cityId, uint modelId, string searchText, string highlightTag = "strong", uint versionId = 0, ushort topCount = 5)
        {
            BikewaleElasticDTO.QuestionSearchWrapperDTO searchResponse = null;
            try
            {
                cityId = cityId == 0 ? uint.Parse(BWConfiguration.Instance.DefaultCity) : cityId;
                if (!string.IsNullOrEmpty(searchText) && modelId > 0)
                {
                    BikeWaleElasticEntities.QuestionSearchWrapper searchResult = _questions.GetQuestionSearch(modelId, searchText, highlightTag, versionId, topCount, cityId);
                    if (searchResult != null)
                    {
                        searchResponse = QuestionMapper.Convert<BikeWaleElasticEntities.QuestionSearchWrapper, BikewaleElasticDTO.QuestionSearchWrapperDTO>(searchResult);
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                ErrorClass.LogError(ex, string.Format("ArgumentNullException:  Bikewale.Service.Controllers.QuestionAndAnswers.GetQuestionList() : Model : {0},SearchText : {1}", modelId, searchText));

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception:  Bikewale.Service.Controllers.QuestionAndAnswers.GetQuestionList() : Model : {0}, CityId : {1} ,  SearchText : {2}", modelId, cityId, searchText));
                return InternalServerError();
            }

            return Ok(searchResponse);
        }


    }
}
