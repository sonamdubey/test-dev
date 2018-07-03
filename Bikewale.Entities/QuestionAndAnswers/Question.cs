using Bikewale.Entities.Customer;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 11 June 2018
    /// Description: Entity to hold information about a Question.
    /// Modified By : Deepak Israni on 25 June 2018
    /// Description : Moved common properties to QuestionBase.
    /// </summary>
    public class Question : QuestionBase
    {
        public IEnumerable<Answer> Answers { get; set; }
    }
}
