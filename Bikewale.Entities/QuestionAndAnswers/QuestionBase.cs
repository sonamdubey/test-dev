using Bikewale.Entities.Customer;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 25 June 2018
    /// Description : QuestionBase entity to hold the basic properties of a question.
    /// </summary>
    public class QuestionBase
    {
        public Guid? Id { get; set; }
        public string Text { get; set; }
        public DateTime AskedOn { get; set; }
        public CustomerEntityBase AskedBy { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public uint ModelId { get; set; }
        public string QuestionAge { get; set; }

    }
}
