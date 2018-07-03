using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.QuestionAndAnswers
{
    public interface IQuestionsCacheRepository
    {
        IEnumerable<string> GetQuestionIdsByModelId(uint modelId);
        uint GetQuestionCountByModelId(uint modelId);
    }
}
