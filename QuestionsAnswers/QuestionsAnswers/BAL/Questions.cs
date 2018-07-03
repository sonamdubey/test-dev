using QuestionsAnswers.Cache;
using QuestionsAnswers.DAL;
using QuestionsAnswers.Entities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace QuestionsAnswers.BAL
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : BAL layer for Questions.
    /// </summary>
    public class Questions : IQuestions
    {
        private readonly IQuestionsRepository _objIQuestionsRepository;
        private readonly IQuestionsCacheRepository _objIQuestionsCacheRepository;

        public Questions(IQuestionsRepository objIQuestionsRepository, IQuestionsCacheRepository objIQuestionsCacheRepository)
        {
            _objIQuestionsRepository = objIQuestionsRepository;
            _objIQuestionsCacheRepository = objIQuestionsCacheRepository;
        }


        /// <summary>
        /// Created by : Snehal Dange on 7th June 2018
        ///  Desc : Method created to fetch all the questions for opr `Manage questions section` 
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public QuestionResult GetQuestions(QuestionsFilter questionFilters, byte startIndex, byte recordCount, byte applicationId)
        {
            QuestionResult objQuestions = null;
            if (questionFilters != null)
            {
                try
                {
                    objQuestions = _objIQuestionsRepository.GetQuestions(questionFilters, startIndex, recordCount, applicationId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objQuestions;

        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 11 June 2018
        /// Description : BAL for Moderating the Question with Id = {QuestionId}
        /// </summary>
        /// <param name="QuestionId">Id of the question to be moderated</param>
        /// <param name="ModeratedBy">Id of the user who is moderating the question</param>
        /// <param name="Action">Action to be performed on the question e.g. `Approve`, `Reject` etc.</param>
        /// <returns></returns>
        public bool ModerateQuestion(string questionId, uint moderatedBy, EnumModerationActionType action, EnumRejectionReasons? rejectionReason)
        {
            bool isModeratedSuccessfully = false;
            try
            {
                switch (action)
                {
                    case EnumModerationActionType.Approve:
                        isModeratedSuccessfully = Approve(questionId, moderatedBy);
                        break;
                    case EnumModerationActionType.Reject:
                        isModeratedSuccessfully = Reject(questionId, moderatedBy, rejectionReason ?? default(int));
                        break;
                    default:
                        // `action` is an unsupported Moderation Type
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return isModeratedSuccessfully;
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 11 June 2018
        /// Description : BAL Method to Approve the question moderated by the internal user
        /// </summary>
        /// <param name="questionId">Id of the question to be approved</param>
        /// <param name="moderatorId">Id of the user Approving the question from the OPR</param>
        /// <returns></returns>
        private bool Approve(string questionId, uint moderatorId)
        {
            bool isApproved = false;
            try
            {
                isApproved = _objIQuestionsRepository.ApproveQuestion(questionId, moderatorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isApproved;
        }


        /// <summary>
        /// Created By : Deepak Israni on 11 June 2018
        /// Description: BAL function to save question in Database
        /// </summary>
        /// <param name="inputQuestion"></param>
        /// <returns></returns>
        public Guid? SaveQuestions(Question inputQuestion, ushort platformId, ushort applicationId, ushort sourceId)
        {
            try
            {
                Guid? questionId = Guid.NewGuid();
                inputQuestion.Id = questionId;

                if (!_objIQuestionsRepository.SaveQuestion(inputQuestion, platformId, applicationId, sourceId))
                {
                    questionId = null;
                }

                return questionId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 13 June 2018
        /// Description : BAL Method to Reject the question moderated by the internal user
        /// </summary>
        /// <param name="questionId">Id of the question to be rejected</param>
        /// <param name="moderatorId">Id of the user Rejecting the question from the OPR</param>
        /// <returns></returns>
        private bool Reject(string questionId, uint moderatorId, EnumRejectionReasons rejectionReason)
        {
            bool isRejected = false;
            try
            {
                isRejected = _objIQuestionsRepository.RejectQuestion(questionId, moderatorId, rejectionReason);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRejected;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 14 June 2018
        /// Description : Function to Update Tags related to a particular question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="moderatorId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool UpdateQuestionTags(string questionId, uint moderatorId, List<uint> oldTags, List<string> newTags)
        {
            bool isUpdateSuccessful = false;
            try
            {
                isUpdateSuccessful = _objIQuestionsRepository.UpdateQuestionTags(questionId, moderatorId, oldTags, newTags);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdateSuccessful;
        }

        /// <summary>
        ///  Created by : Snehal Dange on 20th June 2018
        ///  Desc :  Created method to increase the answer count in questions table
        /// </summary>
        /// <param name="answerObj"></param>
        /// <param name="sourceId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public bool SaveQuestionAnswer(Answer answerObj, ushort platformId, ushort sourceId)
        {
            bool isSuccess = false;
            try
            {
                if (answerObj != null && !string.IsNullOrEmpty(answerObj.QuestionId) && !String.IsNullOrEmpty(answerObj.Text))
                {
                    isSuccess = _objIQuestionsRepository.SaveQuestionAnswer(answerObj, platformId, sourceId);
                    if (isSuccess)
                    {
                        isSuccess = _objIQuestionsRepository.IncreaseAnswerCount(answerObj.QuestionId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;

        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : BAL function to get data of all the question ids.
        /// </summary>
        /// <param name="questionIds"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds)
        {
            List<Question> questions = null;

            try
            {
                if (questionIds != null)
                {
                    if (questionIds.Count() <= 10)
                    {
                        questions = _objIQuestionsCacheRepository.GetQuestionDataByQuestionIds(questionIds) as List<Question>;
                    }
                    else
                    {
                        questions = new List<Question>();

                        IEnumerable<List<string>> splitQuestionIds = splitList<string>(questionIds as List<string>);

                        IEnumerable<Question> batchQuestions = null;
                        foreach (List<string> ids in splitQuestionIds)
                        {
                            batchQuestions = _objIQuestionsCacheRepository.GetQuestionDataByQuestionIds(questionIds);
                            questions.AddRange(batchQuestions);
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            return questions;
        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Private function to split a list into a list of lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locations"></param>
        /// <param name="splitSize"></param>
        /// <returns></returns>
        private IEnumerable<List<T>> splitList<T>(List<T> locations, int splitSize = 10)
        {
            int length = locations.Count;

            for (int i = 0; i < length; i += splitSize)
            {
                yield return locations.GetRange(i, Math.Min(splitSize, length - i));
            }
        }
    }
}
