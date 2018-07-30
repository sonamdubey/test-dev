using Bikewale.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 25th June 2018
    /// Description: VM for Ask Question Popup
    /// </summary>
    public class AskQuestionPopupVM
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public GAPages GAPageType { get; set; }
    }
}
