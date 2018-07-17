using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Entity.QnA.Question;
using QuestionsAnswers.Entities;

namespace BikewaleOpr.Interface.QnA
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : Interface for `Questions` BAL in OPR
    /// Modified by : Snehal Dange on 20th June 2018
    /// Desc  : Created SaveQuestionAnswer()
    /// Modified by : Sanskar Gupta on 28 June 2018
    /// Description : Added an extra parameter `sendEmail` in order to know whether the question moderation email should be sent to the user or not.
    /// </summary>
    public interface IQuestionsBAL
    {
        QuestionModerationResponse ModerateQuestion(string questionId, ModerateQuestionEntity moderateQuestionEntity, EnumModerationActionType action, bool sendEmail);

        Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsResponse UpdateQuestionTags(string questionId, Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity updateQuestionTagsEntity);

        bool SaveQuestionAnswer(Answer answerEntity, ushort platformId, ushort sourceId, string clientIp);

        BikeModelData GetBikeModelDataForQuestion(string questionId);
    }
}
