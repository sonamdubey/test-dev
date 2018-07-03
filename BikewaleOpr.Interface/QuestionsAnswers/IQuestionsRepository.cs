
using BikewaleOpr.Entity.QnA;
using QuestionsAnswers.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.QuestionsAnswers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 16 Jun 2018
    /// Description :   Interface for Questions Repository
    /// Modified By : Snehal Dange on 20th June 2018
    /// Desc : IncreaseAnswerCount for modelquestionmapping table
    /// Modified by : Sanskar Gupta on 26 June 2018
    /// Description : Added function `GetBikeModelDataForQuestion(string questionId)`
    /// Modified by : Sanskar Gupta on 27 June 2018
    /// Description : Added function `GetQuestionIdsByModelId(uint modelId)`
    /// </summary>
    public interface IQuestionsRepository
    {
        bool PublishModelQuestion(string questionId);
        bool UpdateQuestionTags(string questionId, uint modelid);
        bool IncreaseAnswerCount(string questionId);
        BikeModelData GetBikeModelDataForQuestion(string questionId);
        IEnumerable<string> GetQuestionIdsByModelId(uint modelId);
    }
}
