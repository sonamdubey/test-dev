using Bikewale.Entities.QuestionAndAnswers;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.QuestionAndAnswers
{
    /// <summary>
    /// Modified by : Snehal Dange on 7th August 2018
    /// Desc : Added GetHashQuestionMapping
    /// Modified by: Dhruv Joshi
    /// Dated: 10th August 2018
    /// Description: Changed return type of GetHashQuestionMappingByModelId to wrapper class containing two hashtables
    /// </summary>
    public interface IQuestionsCacheRepository
    {
        IEnumerable<string> GetQuestionIdsByModelId(uint modelId);
        uint GetQuestionCountByModelId(uint modelId);
        IEnumerable<string> GetUnansweredQuestionIdsByModelId(uint modelId);
        HashQuestionIdMappingTables GetHashQuestionMapping(uint modelId);        
    }
}
