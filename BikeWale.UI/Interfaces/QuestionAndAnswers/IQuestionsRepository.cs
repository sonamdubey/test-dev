using Bikewale.Entities.QuestionAndAnswers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 14 June 2018
    /// Description: Interface for Questions Repository.
    /// Modified by : Snehal Dange on 7th Aug 2018
    /// Desc : Added GetHashQuestionMappingByModelId
    /// Modified by: Dhruv Joshi
    /// Dated: 10th August 2018
    /// Description: Changed return type of GetHashQuestionMappingByModelId to wrapper class containing two hashtables
    /// </summary>
    public interface IQuestionsRepository
    {
        void StoreQuestionModelMapping(Guid? questionId, uint modelId);
        IEnumerable<String> GetUnansweredQuestionIdsByModelId(uint modelId);
        HashQuestionIdMappingTables GetHashQuestionMappingByModelId(uint modelId);
        IEnumerable<string> GetQuestionIdsByModelId(uint modelId);
    }
}
