using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Feedback
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 21 Jan 2015
    /// </summary>
    public interface IFeedback
    {
        bool SaveCustomerFeedback(string feedbackComment, ushort feedbackType, ushort platformId, string pageUrl);
    }
}
