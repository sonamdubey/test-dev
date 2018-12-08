using AutoMapper;
using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Utility;
using Elasticsearch.Net;
using log4net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BikeWaleElasticEntities = Bikewale.Entities.QuestionAndAnswers.ElasticSearch;

namespace Bikewale.BAL.QuestionAndAnswers
{
    public class Questions : IQuestions
    {

        #region Class Level Variables
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly QuestionsAnswers.BAL.IQuestions _objQNAQuestions = null;
        private readonly IQuestionsRepository _objQuestionsRepository = null;
        private readonly IQuestionsCacheRepository _objQuestionsCacheRepository = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;

        private readonly Random randomizer = null;
        static ILog _logger = LogManager.GetLogger("QuestionAnswerLogger");
        private readonly ElasticClient _client;
        private readonly ushort _infoTopCount = 1;
        private static string _qnaIndexTypeName = "questiondocument";
        private static string _qnaIndexName = BWConfiguration.Instance.QuestionIndex;
        private static string _qnaSuggestionName = "Qna_Suggestion";
        private static IDictionary<string, string> _questionTypeIndexMapping = new Dictionary<string, string>()
        {
            {"EMI", BWConfiguration.Instance.BikeModelPriceIndex},
            {"Price", BWConfiguration.Instance.BikeModelPriceIndex},
            {"Mileage", BWConfiguration.Instance.BikeModelIndex},
        };
        private static IDictionary<string, string> _questionTypeDocumentMapping = new Dictionary<string, string>()
        {
            {"EMI", "modelpricedocument"},
            {"Price", "modelpricedocument"},
            {"Mileage", "bikemodeldocument"},
        };
        #region ES Document Fields
        private static string _bikeModelModelId = "bikeModel.modelId";
        private static string _bikeModelMileage = "topVersion.mileage";
        private static string _qnaQuestion = "question";
        private static string _qnaPageUrl = "pageUrl";
        private static string _qnaAnswerCount = "answerCount";
        private static string _qnaAnswer = "answers";
        private static string _qnaModelId = "modelId";
        private static string _qnaQuestionType = "question.questionType";
        private static string _priceDocumentModelId = "bikeModel.modelId";
        private static string _priceDocumentCityId = "city.cityId";
        private static string _priceDocumentVersionExshowroom = "versionPrice.exshowroom";
        private static string _priceDocumentVersionOnroad = "versionPrice.onroad";
        private static string _priceDocumentVersionVersionId = "versionPrice.versionId";
        #endregion
        #endregion

        #region Constructor
        public Questions(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            QuestionsAnswers.BAL.IQuestions objQNAQuestions,
            IQuestionsRepository objQuestionsRepository,
            IQuestionsCacheRepository objQuestionsCacheRepository, ICityCacheRepository cityCacheRepo)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objQNAQuestions = objQNAQuestions;
            _objQuestionsRepository = objQuestionsRepository;
            _objQuestionsCacheRepository = objQuestionsCacheRepository;
            _cityCacheRepo = cityCacheRepo;
            _client = ElasticSearchInstance.GetInstance();
            randomizer = new Random();
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Created By : Deepak Israni on 12 June 2018
        /// Description: Checks if customer exists and if not creates a new customer entity.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private CustomerEntityBase GetCustomerEntity(string customerName, string mobile, string email)
        {
            CustomerEntity objCust = null;

            if (!_objAuthCustomer.IsRegisteredUser(email, mobile))
            {
                objCust = new CustomerEntity() { CustomerName = customerName, CustomerEmail = email, CustomerMobile = mobile, ClientIP = "" };
                objCust.CustomerId = _objCustomer.Add(objCust);
            }
            else
            {
                objCust = _objCustomer.GetByEmailMobile(email, mobile);

                objCust.CustomerName = customerName;
                objCust.CustomerEmail = !String.IsNullOrEmpty(email) ? email : objCust.CustomerEmail;
                objCust.CustomerMobile = mobile;

                _objCustomer.Update(objCust);
            }

            return objCust;
        }

        /// <summary>
        /// Created By : Deepak Israni on 13 June 2018
        /// Description: Automapper to convert Bikewale Questions entity to QuestionAndAnswers Question entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private QuestionsAnswers.Entities.Question ConvertToQNAQuestionEntity(Question entity)
        {
         
            return Mapper.Map<Question, QuestionsAnswers.Entities.Question>(entity);
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description: Automapper to convert QuestionAndAnswers Questions entity to Bikewale Question entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IEnumerable<Question> ConvertToBikewaleQuestionEntity(IEnumerable<QuestionsAnswers.Entities.Question> entity)
        {
            
            return Mapper.Map<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(entity);
        }


        #endregion

        /// <summary>
        /// Created By : Deepak Israni on 12 June 2018
        /// Description: BAL function to validate inputs and process question data before sending to QnA for saving.
        /// </summary>
        /// <param name="inputQuestion"></param>
        /// <param name="platformId"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public Guid? SaveQuestion(Question inputQuestion, ushort platformId, ushort sourceId, string clientIp)
        {
            ushort applicationId = 2;
            Guid? questionId = null;

            try
            {
                CustomerEntityBase inpCust = inputQuestion.AskedBy;

                #region Input Validation & Sanitization
                if (String.IsNullOrEmpty(inputQuestion.AskedBy.CustomerEmail)
                    || String.IsNullOrEmpty(inputQuestion.Text)
                    || String.IsNullOrEmpty(inputQuestion.AskedBy.CustomerName)
                    || (!String.IsNullOrEmpty(inputQuestion.AskedBy.CustomerEmail) && !Validate.ValidateEmail(inputQuestion.AskedBy.CustomerEmail)))
                {
                    return questionId;
                }
                inputQuestion.Text = StringHtmlHelpers.RemoveHtmlWithSpaces(inputQuestion.Text);

                #endregion

                #region Generation of Customer ID and Masking of Customer Name


                if (!String.IsNullOrEmpty(inpCust.CustomerName))
                {
                    inpCust = GetCustomerEntity(inpCust.CustomerName, inpCust.CustomerMobile, inpCust.CustomerEmail);

                    //Commented Code below for when Customer Name Input is not empty.
                    //string maskedName = inpCust.CustomerEmail.Split('@')[0];
                    //int nameIndex = (maskedName.Length / 2) - 1;

                    //inpCust.CustomerName = StringHelper.MaskUserName(maskedName, nameIndex, 3);
                }
                else
                {
                    return questionId;
                }

                inputQuestion.AskedBy = inpCust;
                #endregion

                #region Storing Question and Maintaining Mapping
                QuestionsAnswers.Entities.ClientInfo clientInfo = new QuestionsAnswers.Entities.ClientInfo()
                {
                    ApplicationId = applicationId,
                    PlatformId = platformId,
                    SourceId = sourceId,
                    ClientIp = clientIp

                };

                questionId = _objQNAQuestions.SaveQuestions(Mappers.Convert<Question, QuestionsAnswers.Entities.Question>(inputQuestion), clientInfo);

                if (questionId != null)
                {
                    _objQuestionsRepository.StoreQuestionModelMapping(questionId, inputQuestion.ModelId);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.SaveQuestion. Platform Id: {0}, Source Id: {1}, Input: {2}", platformId, sourceId, JsonConvert.SerializeObject(inputQuestion)));
            }

            return questionId;
        }



        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Function to get question ids for a certain model.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<string> GetQuestionIdsByModelId(uint modelId, ushort pageNo, ushort pageSize)
        {
            IEnumerable<string> questions = null;

            try
            {
                if (pageNo > 0)
                {
                    IEnumerable<string> questionList = null;
                    questionList = _objQuestionsCacheRepository.GetQuestionIdsByModelId(modelId);
                    if (questionList != null)
                    {
                        List<string> _questions = questionList.ToList();

                        int totalRecords = _questions.Count();
                        int recordsSoFar = (pageNo - 1) * pageSize;
                        int remainingRecords = totalRecords - recordsSoFar;

                        if (remainingRecords > 0)
                        {
                            questions = _questions.GetRange(recordsSoFar, Math.Min(pageSize, remainingRecords));
                        }
                        else
                        {
                            return questions;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionIdsByModelId. Model Id: {0}, Page No.: {1}, Page Size: {2}", modelId, pageNo, pageSize)); ;
            }

            return questions;
        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Get number of answered questions for a model.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public uint GetQuestionCountByModelId(uint modelId)
        {
            uint questionCount = 0;
            try
            {
                questionCount = _objQuestionsCacheRepository.GetQuestionCountByModelId(modelId);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionCountByModelId, Model Id: {0}", modelId));
            }
            return questionCount;
        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Get all the question data for a model id from the QNA service.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionDataByModelId(uint modelId, ushort pageNo, ushort recordSize)
        {
            IEnumerable<Question> questions = null;

            try
            {
                IEnumerable<string> questionIds = GetQuestionIdsByModelId(modelId, pageNo, recordSize);

                if (questionIds != null)
                {
                    questions = Mappers.Convert<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(_objQNAQuestions.GetQuestionDataByQuestionIds(questionIds));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionDataByModelId, Model Id: {0}, Page No: {1}, Record Size: {2}", modelId, pageNo, recordSize));
            }

            return questions;
        }


        /// <summary>
        /// Created By : Deepak Israni on 25 June 2018
        /// Description : Function to get list of question-answer pair for a certain model.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordSize"></param>
        /// <returns></returns>
        public IEnumerable<QuestionAnswer> GetQuestionAnswerDataByModelId(uint modelId, ushort pageNo, ushort recordSize)
        {
            ICollection<QuestionAnswer> questionanswer = null;
            try
            {
                IEnumerable<Question> questions = GetQuestionDataByModelId(modelId, pageNo, recordSize);
                if (questions != null)
                {
                    QuestionAnswer qa = null;
                    questionanswer = new List<QuestionAnswer>();
                    foreach (Question question in questions)
                    {
                        qa = new QuestionAnswer()
                        {
                            Question = question,
                            Answer = question.Answers.FirstOrDefault(),
                            AnswerCount = question.AnswerCount,
                            Url = string.Format("{0}-{1}/", question.BaseUrl, GetQuestionIdHashMapping(question.Id.ToString(), modelId, EnumQuestionIdHashMappingChoice.QuestionIdToHash))
                        };

                        questionanswer.Add(qa);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionAnswerDataByModelId, Model Id: {0}, Page No: {1}, Record Size: {2}", modelId, pageNo, recordSize));
            }

            return questionanswer;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 25 June 2018
        /// Description : Function to return question list alongwith the url of previous and the next pages
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Entities.QuestionAndAnswers.Questions GetQuestionsByModelId(uint modelId, ushort pageNo, ushort pageSize)
        {
            try
            {
                uint totalQuestions = GetQuestionCountByModelId(modelId);
                Entities.QuestionAndAnswers.Questions questions = new Entities.QuestionAndAnswers.Questions();
                questions.QuestionList = GetQuestionAnswerDataByModelId(modelId, pageNo, pageSize);
                FormatQuestionsAnswers(questions.QuestionList);

                if (pageNo > 1)
                {
                    questions.PrevPageURL = string.Format("/questions-and-answers/page/{0}/", pageNo - 1);
                }
                double noOfAvailablePages = Math.Ceiling((double)totalQuestions / pageSize);
                if (questions.QuestionList != null && pageNo < noOfAvailablePages)
                {
                    questions.NextPageURL = string.Format("/questions-and-answers/page/{0}/", pageNo + 1);

                }
                return questions;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionsByModelId, Model Id: {0}, Page No: {1}, Record Size: {2}", modelId, pageNo, pageSize));
                return null;
            }
        }

        /// <summary>
        /// Format answers stripped text
        /// </summary>
        /// <param name="questionAnsList"></param>
        private void FormatQuestionsAnswers(IEnumerable<Entities.QuestionAndAnswers.QuestionAnswer> questionAnsList)
        {
            try
            {
                if (questionAnsList != null)
                {
                    int trimLength = 150;
                    foreach (var quesAnsObj in questionAnsList)
                    {
                        Answer answerValue = quesAnsObj.Answer;
                        QuestionBase questionValue = quesAnsObj.Question;

                        questionValue.QuestionAge = FormatDate.GetTimeSpan(questionValue.AskedOn);
                        answerValue.AnswerAge = FormatDate.GetTimeSpan(answerValue.AnsweredOn);

                        answerValue.StrippedText = (!string.IsNullOrEmpty(answerValue.Text) && (answerValue.Text.Length > trimLength) ? answerValue.Text.Substring(0, trimLength) : string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.FormatQuestionsAnswers()");
            }
        }

        public QuestionAnswerWrapper GetQuestionAnswerList(uint modelId, ushort pageNo, ushort recordSize)
        {
            QuestionAnswerWrapper questionAnswerWrapper = null;
            try
            {
                QuestionAnswerWrapper quesAnsObj = new QuestionAnswerWrapper();
                quesAnsObj.QuestionList = GetQuestionAnswerDataByModelId(modelId, pageNo, recordSize);
                quesAnsObj.TotalAnsweredQuestions = GetQuestionCountByModelId(modelId);
                if (quesAnsObj.QuestionList != null && quesAnsObj.TotalAnsweredQuestions > 0)
                {
                    questionAnswerWrapper = quesAnsObj;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.GetQuestionAnswerList()");
            }
            return questionAnswerWrapper;
        }


        /// <summary>
        /// Created By : Deepak Israni on 13 July 2018
        /// Description : Get remaining unanswered questions for a certain model.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, string questionId, int questionLimit)
        {
            IEnumerable<string> questionIds = null;

            try
            {
                List<string> allQuestions = _objQuestionsCacheRepository.GetUnansweredQuestionIdsByModelId(modelId) as List<string>;

                if (allQuestions != null && questionLimit > 0)
                {
                    allQuestions = allQuestions.Count > questionLimit + 1 ? allQuestions.GetRange(0, questionLimit + 1) : allQuestions;
                    allQuestions.Remove(questionId);

                    questionIds = allQuestions.Count > questionLimit ? allQuestions.GetRange(0, questionLimit) : allQuestions;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetRemainingUnansweredQuestionIds. Model Id: {0}, Question Id: {1}", modelId, questionId));
            }

            return questionIds;
        }

        /// <summary>
        /// Created By : Kumar Swapnil on 07 September 2018
        /// Description : Get remaining unanswered questions for a certain emailId.
        /// Modified By : Deepak Israni on 25 September 2018
        /// Description : Changed function to return all question ids when questionLimit is 0.
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, int questionLimit, string emailId)
        {
            IList<string> allQuestions = _objQuestionsCacheRepository.GetUnansweredQuestionIdsByModelId(modelId).ToList();
            List<string> unapprovedAnswerQuestions = _objQNAQuestions.GetUnapprovedAnswerQuestionIds(emailId).ToList();
            try
            {
                for (int i = 0; i < unapprovedAnswerQuestions.Count; i++)
                {
                    if (allQuestions == null)
                        break;
                    if (allQuestions.Contains(unapprovedAnswerQuestions[i]))
                        allQuestions.Remove(unapprovedAnswerQuestions[i]);
                }

                if (questionLimit > 0)
                {
                    allQuestions = allQuestions.Count > questionLimit + 1 ? allQuestions.Take<string>(questionLimit).ToList<string>() : allQuestions;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetRemainingUnansweredQuestionIds. Model Id: {0}", modelId));
            }

            return allQuestions;
        }

        /// <summary>
        /// Created By : Deepak Israni on 13 July 2018
        /// Description : Get remaining unanswered questions.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionLimit"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, string questionId, int questionLimit)
        {
            IEnumerable<Question> questions = null;
            try
            {
                IEnumerable<string> questionIds = GetRemainingUnansweredQuestionIds(modelId, questionId, questionLimit);
                if (questionIds != null)
                {
                    questions = Mappers.Convert<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(_objQNAQuestions.GetQuestionDataByQuestionIds(questionIds));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("QuestionAndAnswers.GetRemainingUnansweredQuestions, Model Id: {0}, Question Id: {1}, Question Limit: {2}", modelId, questionId, questionLimit));
            }

            return questions;
        }

        public IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit, string EmailId)
        {
            IEnumerable<Question> questions = null;
            try
            {
                IEnumerable<string> questionIds = GetRemainingUnansweredQuestionIds(modelId, questionLimit, EmailId);
                if (questionIds != null)
                {
                    questions = Mappers.Convert<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(_objQNAQuestions.GetQuestionDataByQuestionIds(questionIds));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("QuestionAndAnswers.GetRemainingUnansweredQuestions, Model Id: {0}, Question Id: {1}, Question Limit: {2}", modelId, questionLimit));
            }

            return questions;
        }

        /// <summary>
        /// Created by : Snehal Dange on 7th August 2018
        /// Desc : Get the hash-questionGuid mapping
        /// Modified by: Dhruv Joshi
        /// Dated: 10th August 2018
        /// Description: Modified function to handle question-id hash both way mapping
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public string GetQuestionIdHashMapping(string key, uint modelId, EnumQuestionIdHashMappingChoice mappingChoice) //Get both mappings, 0 for questionId to hash and 1 for hash to questionId
        {
            string value = string.Empty;
            try
            {
                HashQuestionIdMappingTables hashQuesMapping = _objQuestionsCacheRepository.GetHashQuestionMapping(modelId);
                if (hashQuesMapping != null)
                {
                    Hashtable mappingTable = null;
                    if (mappingChoice == EnumQuestionIdHashMappingChoice.QuestionIdToHash)
                    {
                        mappingTable = hashQuesMapping.QuestionIdToHashMapping;
                        if (mappingTable != null && mappingTable[key] != null)
                        {
                            value = mappingTable[key].ToString();
                        }
                    }
                    else
                    {
                        mappingTable = hashQuesMapping.HashToQuestionIdMapping;
                        if (mappingTable != null && mappingTable[key] != null)
                        {
                            value = mappingTable[key].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format(" Bikewale.BAL.QuestionAndAnswers.GetHashQuestionMapping:ModelId : {0}", modelId));
            }
            return value;
        }
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 10th August 2018
        /// Description: Get data for specific question
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Question GetQuestionDataByQuestionId(string questionId)
        {
            Question questionData = null;
            try
            {

                IEnumerable<string> questionIdList = new List<string>() { questionId };
                QuestionsAnswers.Entities.Question qnaServiceQuestion = _objQNAQuestions.GetQuestionDataByQuestionIds(questionIdList).FirstOrDefault();
                questionData = Mappers.Convert<QuestionsAnswers.Entities.Question, Question>(qnaServiceQuestion);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.Questions.GetQuestionDataByQuestionId(string)");
            }
            return questionData;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sept 2018
        /// Description :   Returns the Unanswered questions for a model
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="questionLimit"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit)
        {
            IEnumerable<Question> questions = null;
            try
            {
                IEnumerable<string> questionIds = GetRemainingUnansweredQuestionIds(modelId, "", questionLimit);
                if (questionIds != null)
                {
                    questions = Mappers.Convert<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(_objQNAQuestions.GetQuestionDataByQuestionIds(questionIds));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("QuestionAndAnswers.GetRemainingUnansweredQuestions, Model Id: {0}, Question Limit: {1}", modelId, questionLimit));
            }

            return questions;
        }

        /// <summary>
        /// Created By  : Deepak Israni on 25 September 2018
        /// Description : Function to get Question Ids based on the ordering parameter.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="questionLimit"></param>
        /// <param name="emailId"></param>
        /// <param name="ordering"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, int questionLimit, string emailId, QuestionOrdering ordering)
        {
            IEnumerable<string> allQuestions = null;

            try
            {
                switch (ordering)
                {
                    default:
                    case QuestionOrdering.Default:
                        allQuestions = GetRemainingUnansweredQuestionIds(modelId, questionLimit, emailId);
                        break;
                    case QuestionOrdering.Random:
                        allQuestions = GetRemainingUnansweredQuestionIds(modelId, 0, emailId); //Gets all unanswered questions
                        if (allQuestions != null)
                        {
                            allQuestions = GetRandomList<string>(allQuestions.ToList(), questionLimit); //Picks questions at random
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetRemainingUnansweredQuestionIds. Model Id: {0}, Ordering: {1}", modelId, ordering));
            }

            return allQuestions;
        }

        /// <summary>
        /// Created By  : Deepak Israni on 25 September 2018
        /// Description : Function to get Question Data based on the ordering parameter.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="questionLimit"></param>
        /// <param name="emailId"></param>
        /// <param name="ordering"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit, string emailId, QuestionOrdering ordering)
        {
            IEnumerable<Question> questions = null;
            try
            {
                IEnumerable<string> questionIds = GetRemainingUnansweredQuestionIds(modelId, questionLimit, emailId, ordering);
                if (questionIds != null)
                {
                    questions = Mappers.Convert<IEnumerable<QuestionsAnswers.Entities.Question>, IEnumerable<Question>>(_objQNAQuestions.GetQuestionDataByQuestionIds(questionIds));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("QuestionAndAnswers.GetRemainingUnansweredQuestions, Model Id: {0}, Question Id: {1}, Question Limit: {2}, Ordering: {3}", modelId, questionLimit, ordering));
            }

            return questions;
        }

        /// <summary>
        /// Created By  : Deepak Israni on 25 September 2018
        /// Description : Private function to get a random list back.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<T> GetRandomList<T>(IList<T> input, int topCount)
        {
            int inpLength = input.Count, maxIterations = topCount * 2, count = 0, listLen = topCount;

            if (topCount > inpLength)
            {
                return input;
            }

            HashSet<T> items = new HashSet<T>();

            if (inpLength > 0)
            {
                while (listLen > 0 && count < maxIterations)
                {
                    if (items.Add(input[randomizer.Next(0, inpLength)]))
                    {
                        listLen--;
                    }
                    count++;
                }
            }

            //When randomizer exceeds max iterations allowed, send normal list instead.
            if (count == maxIterations && items.Count != topCount)
            {
                return input.Take<T>(topCount);
            }

            return items;
        }

        #region Qna ElasticSearch
        /// <summary>
        /// Created by : Snehal Dange on 17th Oct 2018
        /// Desc : Gets the documents from question index .
        /// Modified by: Dhruv Joshi
        /// Dated: 6th November 2018
        /// Description: Bring BikeInfo Card data along with user questions
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="searchText"></param>
        /// <param name="highlightTag"></param>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public BikeWaleElasticEntities.QuestionSearchWrapper GetQuestionSearch(uint modelId, string searchText, string highlightTag, uint versionId, ushort topCount, uint cityId)
        {
            BikeWaleElasticEntities.QuestionSearchWrapper searchResults = null;
            try
            {
                BikeWaleElasticEntities.QuestionType questionType = GetQuestionType(searchText);
                searchResults = new BikeWaleElasticEntities.QuestionSearchWrapper()
                {
                    Questions = GetUserQuestions(modelId, searchText, highlightTag, versionId, topCount),
                    Bikeinfo = GetBWInfo(questionType, modelId, versionId, cityId)
                };
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionSearch, Model Id: {0}, SeachText: {1} ", modelId, searchText));
            }

            return searchResults;
        }

        private BikeWaleElasticEntities.QuestionType GetQuestionType(string searchText)
        {
            BikeWaleElasticEntities.QuestionType questionType = BikeWaleElasticEntities.QuestionType.User;
            try
            {
                if (_client != null && !string.IsNullOrEmpty(searchText))
                {
                    var questionSearchDescriptor = BuildBikeInfoQuestionSearchDescriptor(searchText, _infoTopCount);
                    if (questionSearchDescriptor != null)
                    {
                        ISearchResponse<BikeWaleElasticEntities.QuestionSearch> questionResult = _client.Search(questionSearchDescriptor);
                        if (questionResult != null && questionResult.Hits != null && questionResult.Hits.Count > 0)
                        {
                            questionType = questionResult.Hits.FirstOrDefault().Source.Question.QuestionType;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetQuestionType, Search text: {0}", searchText));
            }
            return questionType;
        }

        /// <summary>
        /// Modified By : Monika Korrapati on 30 Nov 2018
        /// Description : Added AutoSuggest condition
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="searchText"></param>
        /// <param name="highlightTag"></param>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<BikeWaleElasticEntities.QuestionSearch> GetUserQuestions(uint modelId, string searchText, string highlightTag, uint versionId, ushort topCount)
        {
            double timeTaken = 0;
            IEnumerable<BikeWaleElasticEntities.QuestionSearch> searchResults = null;
            try
            {
                DateTime startTime = DateTime.Now;
                if (_client != null && !string.IsNullOrEmpty(searchText) && modelId > 0) // question will always be related to a particular model
                {
                    var searchDescriptor = BuildQuestionSearchDescriptor(modelId, searchText, highlightTag, versionId, topCount);

                    if (searchDescriptor != null)
                    {
                        ISearchResponse<BikeWaleElasticEntities.QuestionSearch> _result = _client.Search(searchDescriptor);
                        if (_result != null && _result.Hits != null && _result.Hits.Any())
                        {
                            searchResults = ProcessIndexDocuments(_result.Hits);
                        }
                        else if (_result != null && _result.Suggest != null && _result.Suggest.ContainsKey(_qnaSuggestionName) && _result.Suggest[_qnaSuggestionName][0].Options.Any())
                        {
                            searchResults = ProcessSuggestDocuments(_result.Suggest[_qnaSuggestionName][0].Options);
                        }
                        else
                        {
                            _logger.Info(string.Format("0 Results returned by Search Query : {0}", searchText));
                        }
                    }
                    DateTime endTime = DateTime.Now;
                    timeTaken = (endTime - startTime).TotalMilliseconds;
                }
                else
                    throw new ArgumentNullException();
            }
            catch (ElasticsearchClientException ex)
            {
                ErrorClass.LogError(ex, String.Format("ElasticsearchClientException at Bikewale.BAL.QuestionAndAnswers.GetUserQuestions ModelId :{0} , searchText : {1}", modelId, searchText));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetUserQuestions, Model Id: {0}, SeachText: {1} ", modelId, searchText));
            }
            finally
            {
                ThreadContext.Properties["TotalTime"] = timeTaken;
                _logger.Info("GetQuestionSearch");
                ThreadContext.Properties.Remove("TotalTime");
            }
            return searchResults;
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 6th November 2018
        /// Description: Get BikeInfo Card data
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="questionType"></param>
        /// <returns></returns>
        private BikeWaleElasticEntities.BikewaleInfo GetBWInfo(BikeWaleElasticEntities.QuestionType questionType, uint modelId, uint versionId, uint cityId)
        {
            BikeWaleElasticEntities.BikewaleInfo searchResult = null;
            try
            {
                if (_client != null)
                {
                    CityEntityBase selectedCity = null;
                    switch(questionType)
                    {
                        case BikeWaleElasticEntities.QuestionType.Mileage:
                            searchResult = ProcessMileageInfo(questionType, modelId, searchResult);
                            break;
                        case BikeWaleElasticEntities.QuestionType.Price:
                            if (cityId > 0)
                            {
                                var cities = _cityCacheRepo.GetAllCities(EnumBikeType.All);
                                if (cities != null && (selectedCity = cities.FirstOrDefault(c => c.CityId == cityId)) != null)
                                {
                                    searchResult = ProcessPriceInfo(questionType, modelId, versionId, cityId, searchResult);
                                    if (searchResult != null)
                                    {
                                        searchResult.Description = String.Format("Ex-showroom, {0}:", selectedCity.CityName);
                                    }
                                }
                            }
                            break;
                        case BikeWaleElasticEntities.QuestionType.EMI:
                            if (cityId > 0)
                            {
                                var cities = _cityCacheRepo.GetAllCities(EnumBikeType.All);
                                if (cities != null && (selectedCity = cities.FirstOrDefault(c => c.CityId == cityId)) != null)
                                {
                                    searchResult = ProcessPriceInfo(questionType, modelId, versionId, cityId, searchResult);                                    
                                    if(searchResult != null)
                                    { 
                                        if(searchResult.Value.Equals(0))
                                        {
                                            searchResult = ProcessPriceInfo(questionType, modelId, versionId, 1, searchResult);   // use cityId = 1 (Mumbai) if On-Road price for a given city = 0                                
                                        }
                                        searchResult.Value = EMICalculation.SetDefaultEMIDetails(Convert.ToUInt32(searchResult.Value)).EMIAmount;                                    
                                        searchResult.Description = "EMI starting at:";
                                    }
                                }
                            }

                            break;
                    }                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.QuestionAndAnswers.GetBikeInfo, Model Id: {0}", modelId));
            }
            return searchResult;
        }
        private BikeWaleElasticEntities.BikewaleInfo ProcessPriceInfo(BikeWaleElasticEntities.QuestionType questionType, uint modelId, uint versionId, uint cityId, BikeWaleElasticEntities.BikewaleInfo searchResult)
        {
            ICollection<KeyValuePair<string, object>> filters = new List<KeyValuePair<string, object>>();
            filters.Add(new KeyValuePair<string, object>(_priceDocumentModelId, modelId));
            filters.Add(new KeyValuePair<string, object>(_priceDocumentCityId, cityId));

            var queryContainer = BuildQueryContainerDescriptor<Bikewale.ElasticSearch.Entities.ModelPriceDocument, object>(filters, _priceDocumentVersionExshowroom);

            var bikeInfoSearchDescriptor = BuildInfoSearchDescriptor<Bikewale.ElasticSearch.Entities.ModelPriceDocument, object>(questionType, new string[] { _priceDocumentVersionExshowroom, _priceDocumentVersionOnroad, _priceDocumentVersionVersionId }, queryContainer);
            if (bikeInfoSearchDescriptor != null)
            {
                ISearchResponse<Bikewale.ElasticSearch.Entities.ModelPriceDocument> bikeInfoResult = _client.Search(bikeInfoSearchDescriptor);
                if (bikeInfoResult != null && bikeInfoResult.Hits != null && bikeInfoResult.Hits.Count > 0)
                {
                    if(questionType.Equals(BikeWaleElasticEntities.QuestionType.Price))
                    {
                        searchResult = ProcessBikeInfoDocument<Bikewale.ElasticSearch.Entities.ModelPriceDocument, uint>(hits => bikeInfoResult.Hits.First().Source.VersionPrice.First(m => m.VersionId == versionId).Exshowroom, bikeInfoResult.Hits, questionType);
                    }
                    else
                    {
                        searchResult = ProcessBikeInfoDocument<Bikewale.ElasticSearch.Entities.ModelPriceDocument, uint>(hits => bikeInfoResult.Hits.First().Source.VersionPrice.First(m => m.VersionId == versionId).Onroad, bikeInfoResult.Hits, questionType);                        
                    }
                }
            }
            return searchResult;
        }

        private BikeWaleElasticEntities.BikewaleInfo ProcessMileageInfo(BikeWaleElasticEntities.QuestionType questionType, uint modelId, BikeWaleElasticEntities.BikewaleInfo searchResult)
        {
            ICollection<KeyValuePair<string, object>> filters = new List<KeyValuePair<string, object>>();
            filters.Add(new KeyValuePair<string, object>(_bikeModelModelId, modelId));

            var queryContainer = BuildQueryContainerDescriptor<Bikewale.ElasticSearch.Entities.BikeModelDocument, object>(filters, _bikeModelMileage);

            var bikeInfoSearchDescriptor = BuildInfoSearchDescriptor<Bikewale.ElasticSearch.Entities.BikeModelDocument, object>(questionType, new string[] { _bikeModelMileage }, queryContainer);
            if (bikeInfoSearchDescriptor != null)
            {
                ISearchResponse<Bikewale.ElasticSearch.Entities.BikeModelDocument> bikeInfoResult = _client.Search(bikeInfoSearchDescriptor);
                if (bikeInfoResult != null && bikeInfoResult.Hits != null && bikeInfoResult.Hits.Count > 0)
                {
                    searchResult = ProcessBikeInfoDocument<Bikewale.ElasticSearch.Entities.BikeModelDocument, uint>(hits => bikeInfoResult.Hits.First().Source.TopVersion.Mileage, bikeInfoResult.Hits, questionType);
                }
            }
            return searchResult;
        }
        /// <summary>
        /// Created by : Snehal Dange on 17th Oct 2018
        /// Desc : ProcessIndexDocuments methods maps the index document parameters to the BikeSearch entity
        /// Modified by: Dhruv Joshi
        /// Dated: 30th November 2018
        /// Description: Null checks for highlighted query
        /// </summary>
        /// <param name="resultHits"></param>
        /// <returns></returns>
        private IEnumerable<BikeWaleElasticEntities.QuestionSearch> ProcessIndexDocuments(IReadOnlyCollection<IHit<BikeWaleElasticEntities.QuestionSearch>> resultHits)
        {
            IList<BikeWaleElasticEntities.QuestionSearch> searchResults = null;
            try
            {
                searchResults = new List<BikeWaleElasticEntities.QuestionSearch>();
                BikeWaleElasticEntities.QuestionSearch documentSource = null;

                foreach (var document in resultHits)
                {
                    documentSource = document.Source;
                    if (documentSource != null && documentSource.Question != null)
                    {
                        string highlightedText = string.Empty;
                        if (document.Highlights != null && document.Highlights.Any())
                        {
                            var docHighlights = document.Highlights.FirstOrDefault().Value;
                            if (docHighlights != null && docHighlights.Highlights != null && docHighlights.Highlights.Any())
                            {
                                highlightedText = docHighlights.Highlights.FirstOrDefault();
                            }
                        }

                        if (documentSource.Answers != null && documentSource.Answers.Count() > 0)
                        {
                            BikeWaleElasticEntities.Answer ansObj = documentSource.Answers.OrderByDescending(m => m.AnsweredOn).First();
                            ansObj.AnswerAge = FormatDate.GetTimeSpan(ansObj.AnsweredOn);

                            documentSource.Answer = ansObj;
                        }
                        if (!string.IsNullOrEmpty(highlightedText))
                        {
                            documentSource.Question.QuestionText = highlightedText;
                        }
                        searchResults.Add(documentSource);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.ProcessIndexDocuments()");
            }
            return searchResults;
        }

        /// <summary>
        /// Created By : Monika Korrapati on 30 Nov 2018
        /// Description : Returns Autosuggest results
        /// </summary>
        /// <param name="suggestionList"></param>
        /// <returns></returns>
        private IEnumerable<BikeWaleElasticEntities.QuestionSearch> ProcessSuggestDocuments(IReadOnlyCollection<SuggestOption<BikeWaleElasticEntities.QuestionSearch>> suggestionList)
        {
            IList<BikeWaleElasticEntities.QuestionSearch> searchResults = null;
            try
            {
                if (suggestionList != null && suggestionList.Any())
                {
                    searchResults = new List<BikeWaleElasticEntities.QuestionSearch>();
                    BikeWaleElasticEntities.QuestionSearch documentSource = null;
                    foreach (var document in suggestionList)
                    {
                        documentSource = document.Source;
                        if (documentSource != null && documentSource.Question != null)
                        {
                            if (documentSource.Answers != null && documentSource.Answers.Count() > 0)
                            {
                                BikeWaleElasticEntities.Answer ansObj = documentSource.Answers.OrderByDescending(m => m.AnsweredOn).First();
                                ansObj.AnswerAge = FormatDate.GetTimeSpan(ansObj.AnsweredOn);

                                documentSource.Answer = ansObj;
                            }
                            searchResults.Add(documentSource);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.ProcessSuggestDocuments()");
            }
            return searchResults;
        }
        private BikeWaleElasticEntities.BikewaleInfo ProcessBikeInfoDocument<T, U>(Func<IReadOnlyCollection<IHit<T>>, U> resultHitsCallback, IReadOnlyCollection<IHit<T>> resultHits, BikeWaleElasticEntities.QuestionType questionType)
            where T : class
            where U : struct
        {
            BikeWaleElasticEntities.BikewaleInfo searchResults = null;
            try
            {
                U value = resultHitsCallback.Invoke(resultHits);
                if (value.Equals(default(U)))
                {
                    return searchResults;
                }

                searchResults = new BikeWaleElasticEntities.BikewaleInfo();
                searchResults.Type = questionType;
                searchResults.Value = value;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.ProcessBikeInfoDocuments()");
            }
            return searchResults;
        }

        /// <summary>
        /// Created by : Snehal Dange on 17th Oct 2018
        /// Desc : ProcessHighlightQuery() forms the highlight query . Parameters represents how the highlight query needs to be shown
        /// </summary>
        /// <param name="highlightTag">HighLightTag</param>
        /// <returns></returns>
        private Func<HighlightDescriptor<BikeWaleElasticEntities.QuestionSearch>, IHighlight> ProcessHighlightQuery(string highlightTag)
        {
            string hlstartTag = string.Format("<{0} class=\"highlight-text\">", highlightTag);
            string hlEndTag = string.Format("</{0}>", highlightTag);
            Func<HighlightDescriptor<BikeWaleElasticEntities.QuestionSearch>, IHighlight> highLightQuery = new Func<HighlightDescriptor<BikeWaleElasticEntities.QuestionSearch>, IHighlight>(highLight => highLight.Fields(fields => fields
                             .Field(field => field.Question.QuestionText))
                         .PreTags(hlstartTag)
                         .PostTags(hlEndTag));
            return highLightQuery;
        }


        /// <summary>
        /// Created by : Snehal Dange on 17th Oct 2018
        /// Desc : ProcessMustQuery() process the must part of the query : includes fuzziness and the property which needs to be analysed in ES
        /// </summary>
        /// <returns></returns>
        private Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer> ProcessMustQuery(string searchText)
        {
            Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer> mustQuery = null;
            mustQuery = new Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer>(
                must => must.Match
                    (match => match
                        .Field(field => field.Question.QuestionText)
                        .Fuzziness(Fuzziness.Auto)
                        .Query(searchText)));
            return mustQuery;
        }
        
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 30th November 2018
        /// Description: Wrapping queries under dismax        
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer> ProcessDisMaxMustQuery(string searchText)
        {
            Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer> mustQuery = null;
            mustQuery = new Func<QueryContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, QueryContainer>(
                must => must.DisMax(
                    dismax => dismax.Queries(
                        query => query.Match(
                            match => match.Field(
                                field => field.Question.QuestionText)
                               .Fuzziness(Fuzziness.Auto)
                               .Query(searchText)
                               .MinimumShouldMatch("2<70%")
                               .PrefixLength(2)),

                        query => query.ConstantScore(
                            constScore => constScore.Filter(
                                filter => filter.MatchPhrase(
                                    matchPhrase => matchPhrase.Field(
                                        field => field.Question.QuestionText.Suffix("standard_analysed_text"))
                                        .Query(searchText))))
                                        )));
            return mustQuery;
        }

        /// <summary>
        /// Created By : Monika Korrapati on 30 Nov 2018
        /// Description : suggest Query 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="modelId"></param>
        /// <param name="highlightTag"></param>
        /// <returns></returns>
        private Func<SuggestContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, IPromise<ISuggestContainer>> ProcessSuggestQuery(string searchText, string modelId)
        {
            Func<SuggestContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, IPromise<ISuggestContainer>> suggestQuery = null;
            suggestQuery = new Func<SuggestContainerDescriptor<BikeWaleElasticEntities.QuestionSearch>, IPromise<ISuggestContainer>>(
                s => s
                    .Completion(_qnaSuggestionName, completion => completion
                        .Contexts(context => context
                            .Context(_qnaModelId, ctx => ctx.Context(modelId))
                            )
                        .Field(new Field("suggest"))
                        .Size(5)
                        .Prefix(searchText)
                        )
                );
            return suggestQuery;
        }
        #region QnA Search Descriptors

        /// <summary>
        /// Created by : Snehal Dange on 17th Oct 2018
        /// Desc : Func Descriptor for searching data in QnA index. Topcount :  number of records needed to bind on UI
        /// Modified by : Monika Korrapati on 30 Nov 2018
        /// Description : Added Suggest query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="searchText"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>, SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>> BuildQuestionSearchDescriptor(uint modelId, string searchText, string highlightTag, uint versionId, ushort topCount)
        {
            Func<SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>, SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>> searchDescriptor = BuildSourceFilterDescriptor<BikeWaleElasticEntities.QuestionSearch>(_qnaIndexName, _qnaIndexTypeName, new string[] { _qnaQuestion,
                             _qnaAnswer,_qnaAnswerCount,_qnaPageUrl});

            double timeTaken = 0;
            DateTime startTime = DateTime.Now;

            QueryContainer queryContainer = new QueryContainer();

            ICollection<KeyValuePair<string, object>> filters = new List<KeyValuePair<string, object>>();
            filters.Add(new KeyValuePair<string, object>(_qnaModelId, modelId));

            searchDescriptor += searchDesc => searchDesc
                 .Query(query => query
                     .Bool(boolean => boolean
                         .Filter(ProcessAndFilter<BikeWaleElasticEntities.QuestionSearch, object>(queryContainer, filters))
                         .Must(ProcessDisMaxMustQuery(searchText))
                        ))
                 .Take(topCount)
                 .Suggest(ProcessSuggestQuery(searchText, modelId.ToString()))
                 .Highlight(ProcessHighlightQuery(highlightTag));

            DateTime endTime = DateTime.Now;
            timeTaken = (endTime - startTime).TotalMilliseconds;

            _logger.Info(string.Format("Time taken for BuildSearchDescriptor in QnA: {0} ", timeTaken));
            return searchDescriptor;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 Nov 2018
        /// Description :   Builds the Search descriptor for Bike Info
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>, SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>> BuildBikeInfoQuestionSearchDescriptor(string searchText, ushort topCount)
        {
            Func<SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>, SearchDescriptor<BikeWaleElasticEntities.QuestionSearch>> searchDescriptor = BuildSourceFilterDescriptor<BikeWaleElasticEntities.QuestionSearch>(_qnaIndexName, _qnaIndexTypeName, new string[] { "question.questionType" });

            searchDescriptor += searchDesc => searchDesc
                 .Query(query => query
                     .Bool(boolean => boolean
                         .Must(ProcessMustQuery(searchText))
                         .MustNot(q => q.Term(_qnaQuestionType, 1))
                        ))
                 .Take(topCount);

            return searchDescriptor;
        }


        /// <summary>
        /// Created by  :   Dhruv Joshi on 06 Nov 2018
        /// Description :   Build Generic Bike Info search descriptor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="questionType"></param>
        /// <param name="filters"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<T>, SearchDescriptor<T>> BuildInfoSearchDescriptor<T, V>(BikeWaleElasticEntities.QuestionType questionType, string[] fields, Func<QueryContainerDescriptor<T>, QueryContainer> fnQueryContainer)
            where T : class
            where V : class
        {
            string quesType = questionType.ToString(),
                indexName = _questionTypeIndexMapping[quesType],
                documentType = _questionTypeDocumentMapping[quesType];
            Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = BuildSourceFilterDescriptor<T>(indexName, documentType, fields);

            searchDescriptor += searchDesc => searchDesc
                .Query(fnQueryContainer);
            return searchDescriptor;

        }

        private Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryContainerDescriptor<T, V>(IEnumerable<KeyValuePair<string, V>> filters, string mustNotZero)
            where T : class
            where V : class
        {
            QueryContainer queryContainer = new QueryContainer();

            return query => query
                                .Bool(boolean => boolean
                                     .Filter(ProcessAndFilter<T, V>(queryContainer, filters))
                                     .MustNot(q => q.Term(mustNotZero, 0))
                                     );
        }

        private Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryContainerDescriptor<T, V>(IEnumerable<KeyValuePair<string, V>> filters)
            where T : class
            where V : class
        {
            QueryContainer queryContainer = new QueryContainer();

            return query => query
                                .Bool(boolean => boolean
                                     .Filter(ProcessAndFilter<T, V>(queryContainer, filters))
                                     );
        }

        #endregion

        #region Generic Methods
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 6th November 2018
        /// Description: Base Search Descriptor setting index and document name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documentName"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<T>, SearchDescriptor<T>> BuildBaseSearchDescriptor<T>(string indexName, string documentName) where T : class
        {
            Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = null;
            searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(searchDesc => searchDesc
                 .Index(indexName)
                 .Type(documentName)
                 );
            return searchDescriptor;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 Nov 2018
        /// Description :   Builds the Source Filtered Search descriptor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="documentName"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private Func<SearchDescriptor<T>, SearchDescriptor<T>> BuildSourceFilterDescriptor<T>(string indexName, string documentName, string[] fields) where T : class
        {
            Func<SearchDescriptor<T>, SearchDescriptor<T>> searchDescriptor = null;
            searchDescriptor = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
             searchDesc => searchDesc
                 .Index(indexName)
                 .Type(documentName)
                 .Source(BuildSourceFilter<T>(fields)));
            return searchDescriptor;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 Nov 2018
        /// Description :   BUild Source Filter for Descriptor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        private Func<SourceFilterDescriptor<T>, ISourceFilter> BuildSourceFilter<T>(string[] fields)
            where T : class
        {
            Func<SourceFilterDescriptor<T>, ISourceFilter> sourceFilterDesc = null;
            sourceFilterDesc = new Func<SourceFilterDescriptor<T>, ISourceFilter>(sourceFilter => sourceFilter.Includes(i => i.Fields(fields)));
            return sourceFilterDesc;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 Nov 2018
        /// Description :   Add Terms with and operator in a query container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private QueryContainer ProcessAndFilter<T, V>(QueryContainer query, IEnumerable<KeyValuePair<string, V>> filter)
            where T : class
            where V : class
        {
            if (query == null || filter == null)
                throw new ArgumentException();
            QueryContainerDescriptor<T> FDS = new QueryContainerDescriptor<T>();

            foreach (var pair in filter)
            {
                query &= FDS.Term(pair.Key, pair.Value);
            }
            return query;
        }

        #endregion
        #endregion



    }
}
