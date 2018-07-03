using Bikewale.Entities;
using Bikewale.Entities.Pages;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 14 June 2018
    /// Description: VM for Question and Answer slug.
    /// </summary>
    public class QuestionAnswerSlugVM
    {
        public bool IsQAAvailable { get; set; }
        public Platforms Platform { get; set; }
        public uint ModelId { get; set; }
        public string Tags { get; set; }
        public string ViewAllUrl { get; set; }
        public string MakeName { get; set; }
        public string BikeName { get; set; }
        public string ModelName { get; set; }
        public GAPages GAPageType { get; set; }
    }
}
