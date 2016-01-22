using Bikewale.Entities.BikeBooking;
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
        ValidBikeCancellationResponseEntity IsValidCancellation(string bwid, string mobile);
        bool SaveCancellationOTP(string bwId, string mobile, string otp);
        CancelledBikeCustomer VerifyCancellationOTP(string BwId, String Mobile, String OTP);
    }
}
