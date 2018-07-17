
using QuestionsAnswers.Entities;
using System.Collections.Generic;

namespace QuestionsAnswers.DAL
{
    /// <summary>
    ///  Created by : Snehal Dange on 7th June 2018
    ///  Desc : IQuestionsRepository
    ///  Modified by : Sanskar Gupta on 11 June 2018
    ///  Description : Added Interface method `ApproveQuestion()`
    ///  Modified by : Sanskar Gupta on 13 June 2018
    ///  Description : Added Interface method `RejectQuestion()`
    ///  Modified by : Sanskar Gupta on 14 June 2018
    ///  Description : Added Interface method `UpdateQuestionTags()`
    ///  Modified by : Snehal Dange on 19th June 2018
    ///  Desc        : Added SaveQuestionAnswer()
    ///  Modified by : Snehal Dange on 20th June 2018
    ///  Desc        : Added Increase answer count in questions table()
    /// </summary>
    public interface IQuestionsRepository
    {
        QuestionResult GetQuestions(QuestionsFilter questionFilters, byte startIndex, byte recordCount, byte applicationId);
        bool ApproveQuestion(string questionId, uint moderatorId);
        bool SaveQuestion(Question inputQuestion, ushort platformId, ushort applicationId, ushort sourceId);
        bool RejectQuestion(string questionId, uint moderatorId, EnumRejectionReasons? rejectionReason);

        bool UpdateQuestionTags(string questionId, uint moderatorId, List<uint> oldTags, List<string> newTags);
        bool SaveQuestionAnswer(Answer answerObj, ushort platformId, ushort sourceId);
        bool IncreaseAnswerCount(string questionId);
        IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds);
        IEnumerable<Question> GetQuestionDataByQuestionId(string questionId);
        bool SaveQuestion(Question inputQuestion, ClientInfo clientInfo);
        bool SaveQuestionAnswer(Answer answerObj, ClientInfo clientInfo);
    }
}
