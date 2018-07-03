using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.User;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.QuestionsAnswers;
using BikewaleOpr.Models.QuestionsAndAnswers;
using BikewaleOpr.Service.AutoMappers;
using BikeWaleOpr.Common;
using QuestionsAnswers.BAL;
using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.QuestionAndAnswer
{
    /// <summary>
    /// Created by : Snehal Dange on 14th June 2018
    /// Desc : Question Answer Model
    /// </summary>
    public class QuestionAnswerModel
    {
        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IQuestions _questions = null;
        private readonly IQuestionsAndAnswersCache _questionAnswerCache = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionAnswerModel(IBikeMakesRepository makesRepo, IQuestions questions, IQuestionsAndAnswersCache questionAnswerCache)
        {
            _makesRepo = makesRepo;
            _questions = questions;
            _questionAnswerCache = questionAnswerCache;
        }

        public ManageQuestionsVM GetData(QnAFilters filtersObj)
        {
            ManageQuestionsVM objQuestionsVm = null;
            try
            {
                IEnumerable<BikeMakeEntityBase> makes = _makesRepo.GetMakes((ushort)EnumBikeType.All);
                objQuestionsVm = new ManageQuestionsVM();
                objQuestionsVm.CurrentUserId = Convert.ToInt32(CurrentUser.Id);
                objQuestionsVm.Makes = makes;
                objQuestionsVm.Filters = (filtersObj != null) ? filtersObj : new QnAFilters();
                objQuestionsVm.DetailsPopups = new QuestionDetailsBaseVM()
                {
                    Makes = makes
                };
                objQuestionsVm.DetailsPopups.InternalUsers = _questionAnswerCache.GetInternalUsers();

                GetFilteredQuestionsList(objQuestionsVm, filtersObj);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.QuestionAnswerModel: GetData");
            }
            return objQuestionsVm;
        }

        private void GetFilteredQuestionsList(ManageQuestionsVM objQuestionsVm, QnAFilters filtersObj)
        {
            byte defaultPageSize = 25;
            byte pageNumber = 0;
            byte applicationId = 2;
            try
            {
                QuestionsFilter filtersInput = null;
                if (objQuestionsVm != null)
                {

                    filtersInput = (filtersObj != null) ? QuestionAnswerMapper.Convert(filtersObj) : new QuestionsFilter();

                    if (filtersInput != null)
                    {
                        if (!String.IsNullOrEmpty(filtersInput.CustomerEmails))
                        {
                            filtersInput.CustomerEmails = HttpUtility.UrlDecode(filtersInput.CustomerEmails);
                        }
                        if (!String.IsNullOrEmpty(filtersObj.MakeMaskingName))
                        {
                            filtersInput.TagName = filtersObj.MakeMaskingName;
                            if (!String.IsNullOrEmpty(filtersObj.ModelMaskingName))
                            {
                                filtersInput.TagName = filtersObj.ModelMaskingName;
                            }
                        }

                        filtersObj.ModerationStatus = filtersObj.ModerationStatus == default(EnumQuestionStatus) ? EnumQuestionStatus.Pending : filtersObj.ModerationStatus;


                        switch (filtersObj.ModerationStatus)
                        {
                            case EnumQuestionStatus.Pending:
                                filtersInput.AnsweredStatus = null;
                                filtersInput.ModerationStatus = EnumModerationStatus.New;
                                break;
                            case EnumQuestionStatus.Unanswered:
                                filtersInput.ModerationStatus = EnumModerationStatus.Approved;
                                filtersInput.AnsweredStatus = false;
                                break;
                            case EnumQuestionStatus.Answered:
                                filtersInput.ModerationStatus = EnumModerationStatus.Approved;
                                filtersInput.AnsweredStatus = true;
                                break;
                            case EnumQuestionStatus.Rejected:
                                filtersInput.ModerationStatus = EnumModerationStatus.Rejected;
                                filtersInput.AnsweredStatus = null;
                                break;
                            default:
                                filtersInput.AnsweredStatus = null;
                                filtersInput.ModerationStatus = EnumModerationStatus.New;
                                break;
                        }
                        QuestionResult serviceResult = _questions.GetQuestions(filtersInput, pageNumber, defaultPageSize, applicationId);
                        if (serviceResult != null)
                        {
                            objQuestionsVm.TotalRecordCount = serviceResult.TotalRecordCount;
                            objQuestionsVm.QuestionsList = QuestionListMapper(serviceResult.QuestionList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.QuestionAnswerModel: GetFilteredQuestionsList");
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 14th June 2018
        /// Desc :  Mapper created to bind `Question Answer` service results to client side 
        /// </summary>
        /// <param name="serviceQuestionList"></param>
        /// <returns></returns>
        private IEnumerable<BikewaleOpr.Entity.QnA.Question.Question> QuestionListMapper(IEnumerable<QuestionsAnswers.Entities.Question> serviceQuestionList)
        {
            IList<Entity.QnA.Question.Question> questionList = null;
            try
            {
                if (serviceQuestionList != null)
                {
                    questionList = new List<Entity.QnA.Question.Question>();
                    foreach (var qnaServiceObj in serviceQuestionList)
                    {
                        Entity.QnA.Question.Question questionBaseObj = new Entity.QnA.Question.Question();

                        questionBaseObj.Id = qnaServiceObj.Id;


                        questionBaseObj.Text = qnaServiceObj.Text;
                        questionBaseObj.AskedOn = qnaServiceObj.AskedOn;
                        EndUser endUserObj = new EndUser();

                        Customer quesCust = qnaServiceObj.AskedBy;
                        if (quesCust != null)
                        {
                            endUserObj.Id = quesCust.Id;
                            endUserObj.Name = quesCust.Name;
                            endUserObj.Email = quesCust.Email;

                            questionBaseObj.EndUser = endUserObj;
                        }


                        switch (qnaServiceObj.Status)
                        {
                            case EnumModerationStatus.Approved:
                                if (qnaServiceObj.AnswerCount > 0)
                                {
                                    questionBaseObj.Status = EnumQuestionStatus.Answered;
                                }
                                else
                                {
                                    questionBaseObj.Status = EnumQuestionStatus.Unanswered;
                                }
                                break;
                            case EnumModerationStatus.New:
                                questionBaseObj.Status = EnumQuestionStatus.Pending;
                                break;
                            case EnumModerationStatus.Rejected:
                                questionBaseObj.Status = EnumQuestionStatus.Rejected;
                                break;
                            default:
                                break;
                        }

                        //questionBaseObj.Status = (EnumQuestionStatus)Convert.ToUInt16(qnaServiceObj.Status);

                        questionBaseObj.Tags = QuestionAnswerMapper.Convert(qnaServiceObj.Tags);
                        questionBaseObj.TagsCSV = questionBaseObj.Tags != null ? String.Join(",", questionBaseObj.Tags.Select(m => m.Name)) : "";
                        questionBaseObj.AnswerCount = qnaServiceObj.AnswerCount;
                        questionList.Add(questionBaseObj);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.QuestionAnswerModel: QuestionListMapper");
            }
            return questionList;
        }

    }
}