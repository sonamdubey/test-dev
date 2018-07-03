using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 25 June 2018
    /// Description : Entity to hold list of questions alongwith the other properties
    /// </summary>
    public class Questions
    {
        public IEnumerable<QuestionAnswer> QuestionList { get; set; }

        public string PrevPageURL { get; set; }

        public string NextPageURL { get; set; }
    }
}
