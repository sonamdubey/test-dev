using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 13 July 2018
    /// Description : Entity to store question data along with answering url.
    /// </summary>
    public class QuestionUrl
    {
        public Question QuestionData { get; set; }
        public string AnsweringUrl { get; set; }
    }
}
