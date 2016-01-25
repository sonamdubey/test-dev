using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications.MailTemplates;

namespace Bikewale.BAL.BikeBooking
{
    public class BookingCancellation : IBookingCancellation
    {

        private readonly IBookingCancellation bookingCancelRepository = null;

        public BookingCancellation()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBookingCancellation, Bikewale.DAL.BikeBooking.BookingCancellationRepository>();
                bookingCancelRepository = container.Resolve<IBookingCancellation>();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st Jan 2016
        /// Summary :    To check if booking cancellation request is valid or not
        /// </summary>
        /// <returns>request is valid or not</returns>
        public ValidBikeCancellationResponseEntity IsValidCancellation(string bwId, string mobile)
        {
            int responseFlag = 0;
            ValidBikeCancellationResponseEntity response = default(ValidBikeCancellationResponseEntity);
            response = bookingCancelRepository.IsValidCancellation(bwId, mobile);
            switch (response.ResponseFlag)
            {
                case 0:
                    response.IsVerified = false;
                    response.Message = "BWId and Mobile combination is incorrect";
                    break;

                case 1:
                    response.IsVerified = true;
                    response.Message = "BWId and Mobile combination is correct";
                    break;

                case 2:
                    response.IsVerified = false;
                    response.Message = "This booking has been cancelled already";
                    break;
            }

            // Business Logic
            if (response.IsVerified)
            {
                // create OTP and call SP to to save OTP in db
                string otpCode = GetRandomCode(new Random(), 5);
                SaveCancellationOTP(bwId, mobile, otpCode);
            }
            return response;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 22nd Jan 2016
        /// Summary :    Push OTP for bike cancellation
        /// </summary>
        /// <returns>request is valid or not</returns>
        public uint SaveCancellationOTP(string bwId, string mobile, string otp)
        {
            return bookingCancelRepository.SaveCancellationOTP(bwId, mobile, otp);
        }

        public CancelledBikeCustomer VerifyCancellationOTP(string BwId, String Mobile, String OTP)
        {
            CancelledBikeCustomer customer = default(CancelledBikeCustomer);
            return customer;
        }
        //this function generates a random 5 digit code where all the characters are numeric
        string GetRandomCode(Random rnd, int length)
        {
            string charPool = "1234567890098765432112345678900987654321";
            StringBuilder rs = new StringBuilder();

            while (length-- > 0)
                rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return rs.ToString();
        }


        public CancelledBikeCustomer UpdateCancellationFlag(uint pqId)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmCancellation(uint pqId)
        {
            bool isCancelled = false;
            CancelledBikeCustomer objCancellation = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IPriceQuote _objPriceQuote = null;
                    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                    _objPriceQuote = container.Resolve<IPriceQuote>();

                    isCancelled = _objPriceQuote.SaveBookingState(pqId, Entities.PriceQuote.PriceQuoteStates.Cancelled);

                    objCancellation = bookingCancelRepository.UpdateCancellationFlag(pqId);

                    if(objCancellation!=null)
                    {
                        SMSTypes objSMS = new SMSTypes();
                        objSMS.BookingCancallationSMSToUser(objCancellation.CustomerMobile, objCancellation.CustomerName, "BikeBookingCancellation");

                        ComposeEmailBase objEmail = new BookingCancellationTemplate(objCancellation.BWId, objCancellation.TransactionId, objCancellation.CustomerName,
                            objCancellation.CustomerEmail, objCancellation.CustomerMobile, objCancellation.BookingDate, objCancellation.DealerName, objCancellation.BikeName);

                        objEmail.Send(Bikewale.Utility.BWConfiguration.Instance.LocalMail, "Booking Cancellation Request - " + objCancellation.BWId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.BikeBooking.BookingCancellation.CancelBooking");
                objErr.SendMail();
            }
            return isCancelled;
        }
    }
}
