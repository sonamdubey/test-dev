using System;

namespace BikewaleOpr.Entity.QnA
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Filter for QnA OPR page
    /// </summary>

    public class QnAFilters
    {
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string TagName { get; set; }
        public EnumQuestionStatus ModerationStatus { get; set; }
        public DateTime EntryDate { get; set; }
        public string CustomerEmails { get; set; }
    }
}
