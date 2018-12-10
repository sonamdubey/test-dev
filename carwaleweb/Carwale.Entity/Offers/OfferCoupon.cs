using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{
    public class OfferCoupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public bool EnablePayment { get; set; }
        public string PaymentUrl { get; set; }
        public string BookingAmount { get; set; }
        public ulong ResponseId { get; set; }
    }
}
