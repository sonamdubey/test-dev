using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;

namespace Bikewale.Interfaces.BikeBooking
{
    public interface IBookingCancellation
    {
        ValidBikeCancellationResponseEntity IsValidCancellation(string bwid, string mobile);
        uint SaveCancellationOTP(string bwId, string mobile, string otp);
        CancelledBikeCustomer VerifyCancellationOTP(string BwId, string Mobile, string OTP);
        /// <summary>
        /// Added By : Sadhana Upadhyay on 27 Jan 2016
        /// Summary : To get cancellation details
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        CancelledBikeCustomer GetCancellationDetails(uint pqId);
        /// <summary>
        /// Added By : Sadhana Upadhyay on 27 Jan 2016
        /// Summary : To confirm cancellation ob booking
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        bool ConfirmCancellation(uint pqId);
    }
}
