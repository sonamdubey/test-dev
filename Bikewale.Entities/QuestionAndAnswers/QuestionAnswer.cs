using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 25 June 2018
    /// Description : Entity which holds Question-Answer pair.
    /// Modified By : Deepak Israni on 8 August 2018
    /// Description : Added answer count and url properties.
    /// </summary>
    public class QuestionAnswer
    {
        public QuestionBase Question { get; set; }
        public Answer Answer { get; set; }
        public uint AnswerCount { get; set; }
        public string Url { get; set; }
    }
}
