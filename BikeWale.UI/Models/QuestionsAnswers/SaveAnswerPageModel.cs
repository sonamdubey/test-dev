using AutoMapper;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Customer;
using Bikewale.Models.QuestionAndAnswers;
using QuestionsAnswers.BAL;
using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Bikewale.Utility;
namespace Bikewale.Models.QuestionsAnswers
{
    /// <summary>
    ///  CReated by :Snehal Dange on 13th July 2018
    ///  Desc : Model created for save answer flow for question and answer
    /// </summary>
    public class SaveAnswerPageModel
    {
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IQuestions _objQNAQuestions = null;
        private readonly Interfaces.QuestionAndAnswers.IAnswers _objAnswers;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private int _modelId;
        private string _questionId;
        private string _queryString;

        private readonly CustomerEntityBase objCust = null;
        public bool IsDuplicate { get; private set; }
        public bool IsMobile { get; set; }
        public StatusCodes Status { get; private set; }
        public Bikewale.Entities.QuestionAndAnswers.Sources Source { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SaveAnswerPageModel(string querystring, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IQuestions objQNAQuestions, Interfaces.QuestionAndAnswers.IAnswers objAnswers, ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer)
        {
            _modelMaskingCache = modelMaskingCache;
            _objQNAQuestions = objQNAQuestions;
            _objAnswers = objAnswers;
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _queryString = querystring;

            objCust = new CustomerEntityBase();
            ParseQueryString(objCust);
        }

        /// <summary>
        /// Created  by : Snehal Dange on  17th July 2018
        /// Desc : Getdata() to get the answer page when clicked through email for external user 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public SaveUserAnswerVM GetData()
        {
            SaveUserAnswerVM objVM = null;
            try
            {
                if (!string.IsNullOrEmpty(_questionId))
                {
                    BikeModelEntity objBikeDetails = _modelMaskingCache.GetById(_modelId);
                    objVM = new SaveUserAnswerVM();
                    if (objBikeDetails != null)
                    {
                        objVM.BikeMakeModel = objBikeDetails;
                    }

                    Bikewale.Entities.QuestionAndAnswers.QuestionBase objQuestion = GetQuestionDataById(_questionId);
                    if (objQuestion != null)
                    {
                        objVM.Question = objQuestion;
                        objVM.EncryptedUrl = _queryString;
                        objVM.Platform = IsMobile ? Convert.ToUInt16(Platforms.Mobile) : Convert.ToUInt16(Platforms.Desktop);
                        objVM.Source = (ushort)(Source == Entities.QuestionAndAnswers.Sources.Invalid ?Bikewale.Entities.QuestionAndAnswers.Sources.QuestionAnswer_MailTemplate : Source);
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.QuestionsAnswers.GetData, Model Id: {0}", _modelId));
            }
            return objVM;
        }

        /// <summary>
        /// Created by : Snehal Dange on 19th July 2018
        /// Desc :  To check if the user has already submitted answer for a particular question
        /// </summary>
        /// <param name="objCustBase"></param>
        /// <param name="_questionId"></param>
        private void CheckDuplicateAnswerByUser(CustomerEntityBase objCustBase, string _questionId)
        {
            try
            {
                if (_objAuthCustomer.IsRegisteredUser(objCustBase.CustomerEmail, objCustBase.CustomerMobile))
                {
                    CustomerEntity objCust = _objCustomer.GetByEmailMobile(objCustBase.CustomerEmail, objCustBase.CustomerMobile);
                    if (objCust != null)
                    {
                        IsDuplicate = _objAnswers.CheckDuplicateAnswerByUser(_questionId, (uint)objCust.CustomerId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.QuestionsAnswers.GetData, Model Id: {0}", _modelId));
            }
        }


        /// <summary>
        /// Created by : Snehal Dange on 13th July 2018
        /// Desc : Get the questions data by question id
        /// </summary>
        /// <param name="questionId"></param>
        private Bikewale.Entities.QuestionAndAnswers.Question GetQuestionDataById(string questionId)
        {
            IEnumerable<Bikewale.Entities.QuestionAndAnswers.Question> questions = null;
            try
            {
                ICollection<string> questionIds = new Collection<string>();
                questionIds.Add(questionId);
                IEnumerable<Question> questionList = _objQNAQuestions.GetQuestionDataByQuestionIds(questionIds);
                if (questionList != null && questionList.Any())
                {
                    questions = ConvertToBikewaleQuestionEntity(questionList);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.QuestionsAnswers.GetQuestionDataById, Model Id: {0}", _modelId));
            }
            return questions.FirstOrDefault();
        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description: Automapper to convert QuestionAndAnswers Questions entity to Bikewale Question entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IEnumerable<Bikewale.Entities.QuestionAndAnswers.Question> ConvertToBikewaleQuestionEntity(IEnumerable<Question> entity)
        {
            Mapper.CreateMap<Customer, CustomerEntityBase>()
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.Email));

            Mapper.CreateMap<AnswerBase, Bikewale.Entities.QuestionAndAnswers.Answer>();
            Mapper.CreateMap<Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
            Mapper.CreateMap<Question, Bikewale.Entities.QuestionAndAnswers.Question>();

            return Mapper.Map<IEnumerable<Question>, IEnumerable<Bikewale.Entities.QuestionAndAnswers.Question>>(entity);
        }


        /// <summary>
        /// Created by : Snehal Dange on 13th July 2018
        /// Desc : To decode the encrypted query string
        /// </summary>
        /// <param name="queryString"></param>
        private void ParseQueryString(CustomerEntityBase objCust)
        {
            try
            {
                if ((_queryString.Length % 4) != 0) //Check if the querystring has been altered. Redirect to BikeWale.
                {
                    Status = StatusCodes.ContentNotFound;
                }
                else
                {
                    string decodedString = Bikewale.Utility.TripleDES.DecryptTripleDES(_queryString);

                    NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedString);
                    bool isUrlCorrect = (queryCollection != null && !string.IsNullOrEmpty(queryCollection["userName"]) && !string.IsNullOrEmpty(queryCollection["userEmail"]) & !string.IsNullOrEmpty(queryCollection["questionId"]) && !string.IsNullOrEmpty(queryCollection["modelId"]));

                    if (isUrlCorrect)
                    {
                        objCust.CustomerName = queryCollection["userName"];
                        objCust.CustomerEmail = queryCollection["userEmail"];
                        _questionId = queryCollection["questionId"];
                        _modelId = int.Parse(queryCollection["modelId"]);

                        // if query string is correct check if its a duplicate entry
                        CheckDuplicateAnswerByUser(objCust, _questionId);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.QuestionsAnswers.ParseQnAQueryString, Model Id: {0}", _modelId));
            }
        }

    }
}