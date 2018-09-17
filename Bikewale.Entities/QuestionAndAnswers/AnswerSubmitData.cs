using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Sep 2018
    /// Description :   Answer Submit Data for Form submit
    /// </summary>
    public class AnswerSubmitData
    {
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public ushort PlatformId { get; set; }
        public ushort SourceId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
