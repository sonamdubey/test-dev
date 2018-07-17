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
    }
}
