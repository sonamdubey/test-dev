using System;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ESSurveyOptions
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionValue { get; set; }
        public int OptionNumber { get; set; }
    }
}
