using Bikewale.Entities.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeBooking
{
    public interface IBookingCancellation
    {
        CancelledBikeCustomer VerifyCancellationOTP(string BwId, String Mobile, String OTP);
    }
}
