using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale.Models.QuestionAndAnswers
{
    /// <summary>
    /// Modified By : Kumar Swapnil on 23 November 2018
    /// Description : Added SourceName property for server side binding of ga data attributes
    /// </summary>
    public class ThankYouPageVM : ModelBase
    {
        public IEnumerable<QuestionUrl> Questions { get; set; }
        public string BikeName { get; set; }
        public string ReturnUrl { get; set; }
        public ushort Source { get; set; }
        public string SourceName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
    }
}
