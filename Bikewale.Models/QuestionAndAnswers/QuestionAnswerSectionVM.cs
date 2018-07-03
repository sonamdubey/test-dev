using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Pages;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 22 June 2018
    /// Description : ViewModel for QnA Section for Model page.
    /// </summary>
    public class QuestionAnswerSectionVM
    {
        public IEnumerable<QuestionAnswer> Questions { get; set; }
        public String ViewAllURL { get; set; }
        public Platforms Platform { get; set; }
        public uint ModelId { get; set; }
        public string Tags { get; set; }
        public string BikeName { get; set; }
        public uint QACount { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public GAPages GAPageType { get; set; }
    }
}
