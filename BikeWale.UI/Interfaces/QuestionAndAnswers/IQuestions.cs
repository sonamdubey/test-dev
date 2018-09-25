using Bikewale.Entities.QuestionAndAnswers;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 12 June 2018
    /// Description: Interface for Questions BAL.
    /// Modified by : Sanskar Gupta on 25 June 2018
    /// Description : Added function `GetQuestionsByModelId()`
    /// Modified by : Snehal Dange on 27th June 2018
    /// Description : Added function `GetQuestionAnswerList()`
    /// Modified by: Dhruv Joshi
    /// Dated: 10th August 2018
    /// Description: Added GetQuestionByQuestionId(string, uint), GetQuestionIdHashMapping(string, uint, sbyte)
    /// Modified by :   Sumit Kate on 03 Sept 2018
    /// Description :   Added overload method for GetRemainingUnansweredQuestions with modelid and questionLimit
    /// </summary>
    public interface IQuestions
    {
        Guid? SaveQuestion(Question inputQuestion, ushort platformId, ushort sourceId, string clientIp);
        IEnumerable<string> GetQuestionIdsByModelId(uint modelId, ushort pageNo, ushort pageSize);
        uint GetQuestionCountByModelId(uint modelId);
        IEnumerable<Question> GetQuestionDataByModelId(uint modelId, ushort pageNo, ushort recordSize);
        IEnumerable<QuestionAnswer> GetQuestionAnswerDataByModelId(uint modelId, ushort pageNo, ushort recordSize);
        Questions GetQuestionsByModelId(uint modelId, ushort pageNo, ushort pageSize);
        QuestionAnswerWrapper GetQuestionAnswerList(uint modelId, ushort pageNo, ushort recordSize);
        IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, string questionId, int questionLimit);
        IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, int questionLimit, string emailId);
        IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, string questionId, int questionLimit);
        IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit, string emailId);
        Question GetQuestionDataByQuestionId(string questionId);
        string GetQuestionIdHashMapping(string key, uint modelId, EnumQuestionIdHashMappingChoice mappingChoice);
        IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit);
        IEnumerable<string> GetRemainingUnansweredQuestionIds(uint modelId, int questionLimit, string emailId, QuestionOrdering ordering);
        IEnumerable<Question> GetRemainingUnansweredQuestions(uint modelId, int questionLimit, string emailId, QuestionOrdering ordering);
    }
}
