using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    public class TC_PaymentInquiryEntity
    {  
        public int CwOfferId { get; set; }
        public int InquiryId { get; set; }
        public string Payment { get; set; }
        public string CouponCode { get; set; }
        public string BookingDate { get; set; }
    }
}
