using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA.Question
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : Entity for Question Moderation
    /// Modified by : Sanskar Gupta on 26 June 2018
    /// Description : Removed properties `Tag` and `TagPageUrl`
    /// </summary>
    [Serializable]
    public class ModerateQuestionEntity
    {
        public uint ModeratedBy { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string QuestionText { get; set; }

        public QuestionsAnswers.Entities.EnumRejectionReasons? RejectionReasonId { get; set; }
    }
}
