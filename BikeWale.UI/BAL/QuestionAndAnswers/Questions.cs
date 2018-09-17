using AutoMapper;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        #endregion

        #region Constructor
        public Questions(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            QuestionsAnswers.BAL.IQuestions objQNAQuestions,
            IQuestionsRepository objQuestionsRepository,
            IQuestionsCacheRepository objQuestionsCacheRepository)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objQNAQuestions = objQNAQuestions;
            _objQuestionsRepository = objQuestionsRepository;
            _objQuestionsCacheRepository = objQuestionsCacheRepository;
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
            Mapper.CreateMap<CustomerEntityBase, QuestionsAnswers.Entities.Customer>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.CustomerEmail));

            Mapper.CreateMap<Answer, QuestionsAnswers.Entities.AnswerBase>();
            Mapper.CreateMap<Bikewale.Entities.QuestionAndAnswers.Tag, QuestionsAnswers.Entities.Tag>();
            Mapper.CreateMap<Question, QuestionsAnswers.Entities.Question>();

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
            Mapper.CreateMap<QuestionsAnswers.Entities.Customer, CustomerEntityBase>()
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.Email));

            Mapper.CreateMap<QuestionsAnswers.Entities.AnswerBase, Answer>();
            Mapper.CreateMap<QuestionsAnswers.Entities.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
            Mapper.CreateMap<QuestionsAnswers.Entities.Question, Question>();

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
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, int questionLimit, string emailId)
        {
            IList<string> allQuestions = _objQuestionsCacheRepository.GetUnansweredQuestionIdsByModelId(modelId).ToList();
            List<string> unapprovedAnswerQuestions = _objQNAQuestions.GetUnapprovedAnswerQuestionIds(emailId).ToList();
            try
            {
                for (int i = 0; i < unapprovedAnswerQuestions.Count ; i++)
                {
                    if (allQuestions == null)
                        break;
                    if (allQuestions.Contains(unapprovedAnswerQuestions[i]))
                        allQuestions.Remove(unapprovedAnswerQuestions[i]);
                }
                allQuestions = allQuestions.Count > questionLimit + 1 ? allQuestions.Take<string>(questionLimit).ToList<string>() : allQuestions;

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
                if(hashQuesMapping != null)
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
                        if(mappingTable != null && mappingTable[key] != null)
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
                IEnumerable<string> questionIds = GetRemainingUnansweredQuestionIds(modelId,"", questionLimit);
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

    }
}
