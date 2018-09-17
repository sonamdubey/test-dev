using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale.Models.QuestionAndAnswers
{
    public class ThankYouPageVM : ModelBase
    {
        public IEnumerable<QuestionUrl> Questions { get; set; }
        public string BikeName { get; set; }
        public string ReturnUrl { get; set; }
    }
}
