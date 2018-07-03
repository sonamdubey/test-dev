
using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
namespace QuestionsAnswers.BAL
{
    ///<summary>
    ///Modified by : Sanskar Gupta on 11 June 2018
    ///Description : Added Interface method `ModerateQuestion()`
    ///Modified by : Sanskar Gupta on 13 June 2018
    ///Description : Added Interface method `UpdateQuestionTags()`
    ///Modified by : Snehal Dange on 19th June 2018
    ///Desc : 
    ///</summary>
    public interface IQuestions
    {
        QuestionResult GetQuestions(QuestionsFilter questionFilters, byte startIndex, byte recordCount, byte applicationId);

        Guid? SaveQuestions(Question inputQuestion, ushort platformId, ushort applicationId, ushort sourceId);
        bool ModerateQuestion(string questionId, uint userId, EnumModerationActionType action, EnumRejectionReasons? rejectionReason);
        bool UpdateQuestionTags(string questionId, uint moderatorId, List<uint> oldTags, List<string> newTags);
        bool SaveQuestionAnswer(Answer answerObj, ushort platformId, ushort sourceId);
        IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds);

    }
}
