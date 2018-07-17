using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Cache;
using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.Question;
using BikewaleOpr.Interface.QnA;
using BikewaleOpr.Interface.QuestionsAnswers;
using QuestionsAnswers.BAL;
using QuestionsAnswers.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.BAL.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : BAL for Questions
    /// </summary>
    public class QuestionsBAL : IQuestionsBAL
    {
        private readonly IQuestions _questions = null;
        private readonly IQuestionsRepository _questionsRepository = null;

        public QuestionsBAL(IQuestions questions, IQuestionsRepository questionsRepository)
        {
            _questions = questions;
            _questionsRepository = questionsRepository;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 12 June 2018
        /// Description : BAL for moderating a question
        /// </summary>
        public QuestionModerationResponse ModerateQuestion(string questionId, ModerateQuestionEntity moderateQuestion, EnumModerationActionType action, bool sendEmail)
        {
            QuestionModerationResponse questionModerationResponse = null;
            try
            {
                questionModerationResponse = new QuestionModerationResponse
                {
                    questionId = questionId,
                };

                questionModerationResponse.isModeratedSuccessfully = _questions.ModerateQuestion(questionId, moderateQuestion.ModeratedBy, action, moderateQuestion.RejectionReasonId);

                BikeModelData associatedModelData = null;
                if (questionModerationResponse.isModeratedSuccessfully)
                {
                    associatedModelData = GetBikeModelDataForQuestion(questionId);
                    //When question is approved modelquestionmapping table should be updated. IsActive flag is set true
                    if (action == EnumModerationActionType.Approve)
                    {
                        _questionsRepository.PublishModelQuestion(questionId);


                        // Clear cache for question ids for this model id.
                        if (associatedModelData != null)
                        {
                            BwMemCache.ClearQuestionsForModelId(associatedModelData.ModelId); 
                        }
                    }
                    questionModerationResponse.message = string.Format("Successfully changed the status of the question to {0}", action);
                }
                else
                {
                    questionModerationResponse.message = string.Format("An error occured while changing the status of the question to {0}", action);
                }
                #region Send Email to Customer
                if (sendEmail && !string.IsNullOrEmpty(moderateQuestion.UserEmail) && questionModerationResponse.isModeratedSuccessfully)
                {
                    string dedicatedPageUrl = string.Empty, bikeName = string.Empty;
                    string modelPageUrl = string.Format("{0}/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, associatedModelData.MakeMaskingName, associatedModelData.ModelMaskingName);
                    if (associatedModelData != null && associatedModelData.ModelId != 0)
                    {
                        GetDedicatedPageUrl(associatedModelData, ref dedicatedPageUrl, ref bikeName); 
                    }
                    switch (action)
                    {
                        case EnumModerationActionType.Approve:
                            SendEmailQuestionAnswerCustomer.SendQuestionApprovalEmail(moderateQuestion.UserEmail, moderateQuestion.UserName, moderateQuestion.QuestionText, bikeName, dedicatedPageUrl, modelPageUrl);
                            break;
                        case EnumModerationActionType.Reject:
                            SendEmailQuestionAnswerCustomer.SendQuestionRejectionEmail(moderateQuestion.UserEmail, moderateQuestion.UserName, moderateQuestion.QuestionText, bikeName, dedicatedPageUrl);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.ModerateQuestion(QuestionId:{0}, ModeratedBy:{1}, Action:{2})", questionId, moderateQuestion.ModeratedBy, action));
            }
            return questionModerationResponse;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 27 June 2018
        /// Description : Function to populate the fields `dedicatedPageUrl` and `bikeName`
        /// </summary>
        /// <param name="associatedModelData"></param>
        /// <param name="dedicatedPageUrl"></param>
        /// <param name="bikeName"></param>
        private void GetDedicatedPageUrl(BikeModelData associatedModelData, ref string dedicatedPageUrl, ref string bikeName)
        {
            try
            {
                if (associatedModelData != null)
                {
                    bikeName = associatedModelData.BikeName;
                    uint noOfQuestionsForModel = GetQuestionCountForModel(associatedModelData.ModelId);

                    // Populate `dedicatedPageUrl` only when number of questions posted for that model is atleast 2.
                    if (noOfQuestionsForModel >= 2)
                    {
                        dedicatedPageUrl = string.Format("{0}/{1}-bikes/{2}/questions-and-answers/", BWConfiguration.Instance.BwHostUrl, associatedModelData.MakeMaskingName, associatedModelData.ModelMaskingName);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.QuestionsAnswers.GetDedicatedPageUrl()");
            }
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 27 June 2018
        /// Description : Function to get number of questions associated to a particular `modelId`
        /// </summary>
        /// <param name="modelId">ID of the model for which the count of questions is to be calculated</param>
        /// <returns></returns>
        private uint GetQuestionCountForModel(uint modelId)
        {
            uint noOfQuestions=0;
            try
            {
                IEnumerable<string> questionIds = _questionsRepository.GetQuestionIdsByModelId(modelId);
                noOfQuestions = (uint) questionIds.Count();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.GetQuestionCountForModel({0})", modelId));
            }
            return noOfQuestions;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 14 June 2018
        /// Description : Function to update the tags related to a particular question
        /// Modified By : Deepak Israni on 16 June 2018
        /// Description : Added call to DAL function UpdateQuestionTags to update Bikewale's Question-Model mapping table.
        /// </summary>
        /// <param name="updateQuestionTags"></param>
        /// <returns></returns>
        public Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsResponse UpdateQuestionTags(string questionId, Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity updateQuestionTags)
        {
            Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsResponse response = null;
            try
            {
                response = new Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsResponse
                {
                    QuestionId = questionId
                };

                response.IsSuccessful = _questions.UpdateQuestionTags(questionId, updateQuestionTags.ModeratorId, updateQuestionTags.OldTags, updateQuestionTags.NewTags);

                if (response.IsSuccessful)
                {

                    BikeModelData bikeModelData = GetBikeModelDataForQuestion(questionId);

                    // Clear questions cache associated with Old Model ID
                    if(bikeModelData != null)
                    {
                        BwMemCache.ClearQuestionsForModelId(bikeModelData.ModelId);
                    }

                    //Update Bikewale Question-Model mapping table
                    _questionsRepository.UpdateQuestionTags(questionId, updateQuestionTags.ModelId);

                    // Clear questions cache associated with New Model ID
                    BwMemCache.ClearQuestionsForModelId(updateQuestionTags.ModelId);


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.UpdateQuestionTags(QuestionId:{0}, ModeratedBy:{1})", questionId, updateQuestionTags.ModeratorId));
            }
            return response;
        }


        /// <summary>
        /// CReated by : Snehal Dange on 20th June 2018
        /// Desc :  save answer to answer table and update count in bikewale modelquestionmapping table
        /// Modified By : Deepak Israni on 10 July 2018
        /// Description : Change call to different SaveQuestionAnswer function that takes in client ip as input too.
        /// </summary>
        /// <param name="answerEntity"></param>
        /// <returns></returns>
        public bool SaveQuestionAnswer(Answer answerEntity, ushort platformId, ushort sourceId, string clientIp)
        {
            bool issucess = false;
            try
            {
                if (answerEntity != null && answerEntity.QuestionId != null)
                {
                    ClientInfo clientInfo = new ClientInfo()
                    {
                        PlatformId = platformId,
                        SourceId = sourceId,
                        ClientIp = clientIp

                    };

                    issucess = _questions.SaveQuestionAnswer(answerEntity, clientInfo);
                    if (issucess)
                    {
                        _questions.IncreaseAnswerCount(answerEntity.QuestionId);
                        BikeModelData associatedModelData = GetBikeModelDataForQuestion(answerEntity.QuestionId);
                        uint modelId = (associatedModelData != null) ? associatedModelData.ModelId : 0; 
                        #region Send Email to Customer
                        if (!string.IsNullOrEmpty(answerEntity.AskedByEmail))
                        {
                            string dedicatedPageUrl = string.Empty, bikeName = string.Empty, modelPageUrl = string.Empty;
                            if (modelId != 0)
                            {
                                modelPageUrl = string.Format("{0}/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, associatedModelData.MakeMaskingName, associatedModelData.ModelMaskingName);
                                GetDedicatedPageUrl(associatedModelData, ref dedicatedPageUrl, ref bikeName);
                            }
                            SendEmailQuestionAnswerCustomer.SendQuestionAnsweredEmail(answerEntity.AskedByEmail, answerEntity.AskedByName, answerEntity.QuestionText, answerEntity.Text, answerEntity.AnsweredBy.Name, bikeName, dedicatedPageUrl, modelPageUrl);
                        }
                        #endregion
                        issucess = _questionsRepository.IncreaseAnswerCount(answerEntity.QuestionId);

                        if (issucess && modelId != 0)
                        {
                            // Clear Questions cache for the model id
                            BwMemCache.ClearQuestionsForModelId(associatedModelData.ModelId);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.SaveQuestionAnswer() | QuestionId:{0}", answerEntity.QuestionId));
            }
            return issucess;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 26 June 2018
        /// Description : Function to get data for the model associated with a particular `questionId`
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public BikeModelData GetBikeModelDataForQuestion(string questionId)
        {
            BikeModelData bikeModelData = null;
            try
            {
                bikeModelData = _questionsRepository.GetBikeModelDataForQuestion(questionId);
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.GetBikeModelDataForQuestion() | QuestionId:{0}", questionId));
            }

            return bikeModelData;
        }

    }
}
