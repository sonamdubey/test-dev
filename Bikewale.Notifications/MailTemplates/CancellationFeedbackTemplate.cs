using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by Sangram Nandkhile 
    /// Created on: 12 Feb 2016
    /// </summary>
    public class CancellationFeedbackTemplate: ComposeEmailBase
    {
        protected string _bwId = string.Empty;
        protected string _feedbackText = string.Empty;
        public CancellationFeedbackTemplate(string bwId, string feedbackText)
        {
            _bwId = bwId;
            _feedbackText = feedbackText;
        }
        public override string ComposeBody()
        {
            try
            {
                StringBuilder sb = null;
                sb = new StringBuilder();
                sb.Append(string.Format("<div>Dear Team,<br /><div><br /><div>Customer with Booking id :<b>{0}&nbsp;</b>has cancelled booking and following is the feedback we've received.</div></div><div><br /></div><div><b>Feedback:</b></div><div><br />{1}</div></div>", _bwId, _feedbackText));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
