using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.EmailAlerts
{
    public class ImageUploadReminderData
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerKey { get; set; }
        public int InquiryId { get; set; }
        public string CarName { get; set; }
    }
}
