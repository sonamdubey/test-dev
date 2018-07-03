using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr;
using Newtonsoft.Json;
namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : DTO for Question Moderation
    /// Modified by : Sanskar Gupta on 26 June 2018
    /// Description : Removed properties `Tag` and `TagPageUrl`
    /// </summary>
    public class ModerateQuestionDTO
    {
        [JsonProperty("moderatedBy")]
        public uint ModeratedBy { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }

        [JsonProperty("rejectionReasonId")]
        public RejectionReasonsDTO? RejectionReasonId { get; set; }
    }
}
