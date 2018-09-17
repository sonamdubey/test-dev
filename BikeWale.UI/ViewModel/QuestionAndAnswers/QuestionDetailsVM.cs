using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th August 2018
    /// Description: VM for question details page
    /// </summary>
    public class QuestionDetailsVM : ModelBase
    {
        public Question Question { get; set; }
        public uint BikeModelAnsweredQuestions { get; set; }
        public BikeInfoVM BikeInfoWidget { get; set; }
    }
}
