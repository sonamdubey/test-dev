using Bikewale.Entities;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.QuestionAndAnswers;


namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 20th June 2018
    /// Desc :  Question answer view model for dedicated page
    /// Modified by: Dhruv Joshi
    /// Dated: 25th June 2018
    /// Description: Added AskQuestionPopupVM
    /// </summary>
    public class ModelQuestionsAnswersVM : ModelBase
    {
        public QuestionAnswerWrapper QuestionAnswerWrapper { get; set; }
        public GenericBikeInfo MakeModelBase { get; set; }
        public AskQuestionPopupVM AskQuestionPopup { get; set; }
        public Platforms Platform { get; set; }
        public uint ModelId { get; set; }
        public string Tags { get; set; }

    }
}
