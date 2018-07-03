
using System;
namespace QuestionsAnswers.Entities
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : Entity to hold all the properties to be used by the Filter
    /// </summary>
    [Serializable]
    public class QuestionsFilter
    {
        public string TagName { get; set; }
        public EnumModerationStatus ModerationStatus { get; set; }
        public bool? AnsweredStatus { get; set; }
        public string CustomerEmails { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
