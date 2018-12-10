using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    public class OfferPaymentEntity
    {
        public int DealerId { get; set; }
        public int OfferId { get; set; }
        public int AutobizInquiryId { get; set; }
        public string PaymentAmount { get; set; }
        public string CouponCode { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
