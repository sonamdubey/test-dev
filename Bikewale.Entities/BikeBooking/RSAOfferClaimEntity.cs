using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class RSAOfferClaimEntity
    {
        public string BookingNum { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPincode { get; set; }
        public string BikeRegistrationNo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string Comments { get; set; }
        public uint VersionId { get; set; }
        public ushort HelmetId { get; set; }
    }
}
