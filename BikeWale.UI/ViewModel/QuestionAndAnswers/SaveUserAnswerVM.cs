using Bikewale.Entities.BikeData;
using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 13th July 2018
    /// Desc : VM created to save the external users answers
    /// </summary>
    public class SaveUserAnswerVM : ModelBase
    {
        public BikeModelEntity BikeMakeModel { get; set; }
        public QuestionBase Question { get; set; }
        public string EncryptedUrl { get; set; }
        public ushort Platform { get; set; }
        public ushort Source { get; set; }
        public string ReturnUrl { get; set; }
    }
}
