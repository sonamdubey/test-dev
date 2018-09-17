using Bikewale.Entities.QuestionAndAnswers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.QuestionAndAnswers
{
    public class UnansweredQuestionsVM
    {
        public IEnumerable<QuestionUrl> Questions { get; set; }
    }
}
