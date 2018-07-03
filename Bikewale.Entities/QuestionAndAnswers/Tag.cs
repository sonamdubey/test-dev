using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 11 June 2018
    /// Description: Entity to hold information about a tag related to a Question.
    /// </summary>
    public class Tag
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
