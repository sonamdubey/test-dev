using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Customers
{
    public class UserFeedback
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Feedback { get; set; }
        public string UserIp { get; set; }
        public DateTime FeedbackDateTime { get; set; }
        public string Source { get; set; }
        public short FeedbackRating { get; set; }
        public string CarInfo { get; set; }
        public short? SourceId { get; set; }
        public int? ReportId { get; set; }
    }
}
