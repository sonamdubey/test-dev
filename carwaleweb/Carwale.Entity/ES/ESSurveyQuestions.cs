using System;
using System.Collections.Generic;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ESSurveyQuestions
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public string Question { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMultiselect { get; set; }
        public bool IsFreeText { get; set; }
        public int QuestionSetId { get; set; }
        public List<ESSurveyOptions> Options { get; set; }
    }    
}
