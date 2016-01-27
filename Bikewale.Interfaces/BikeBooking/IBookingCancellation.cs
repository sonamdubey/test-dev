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
        uint SaveCancellationOTP(string bwId, string mobile, string otp);
        CancelledBikeCustomer VerifyCancellationOTP(string BwId, string Mobile, string OTP);
        CancelledBikeCustomer UpdateCancellationFlag(uint pqId);
        bool ConfirmCancellation(uint pqId);
    }
}
