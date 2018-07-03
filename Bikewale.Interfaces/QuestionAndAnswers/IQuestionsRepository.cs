using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 14 June 2018
    /// Description: Interface for Questions Repository.
    /// </summary>
    public interface IQuestionsRepository
    {
        void StoreQuestionModelMapping(Guid? questionId, uint modelId);
        IEnumerable<String> GetQuestionIdsByModelId(uint modelId);
    }
}
