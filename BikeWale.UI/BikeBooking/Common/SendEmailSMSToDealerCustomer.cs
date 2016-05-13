using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay
    /// Modified by :   Lucky Rathore on 12 May 2016
    /// Description :   Update SendEmailToDealer() and NewBikePriceQuoteMailToDealerTemplate() Sinature.
    /// </summary>
    public class SendEmailSMSToDealerCustomer
    {
        public static void SendEmailToDealer(string makeModelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath)
        {
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');

                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new NewBikePriceQuoteMailToDealerTemplate(makeModelName, versionName, dealerName, customerName, 
                        customerEmail, customerMobile, areaName, cityName,
                        priceList, totalPrice, offerList, imagePath);
                    objEmail.Send(string.Format("{0} BikeWale Purchase Inquiry - {1} {2}", email, makeModelName, versionName), customerEmail);
                }
            }
        }

        public static void SendEmailToCustomer(string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice,
            string versionName, double dealerLat, double dealerLong, string workingHours)
        {
            ComposeEmailBase objEmail = new NewBikePriceQuoteToCustomerTemplate(bikeName, versionName, bikeImage, dealerEmail, dealerMobileNo,
                organization, address, customerName, priceList, offerList, pinCode, stateName, cityName, totalPrice, dealerLat, dealerLong, workingHours);
        }

        public static void SMSToDealer(string dealerMobile, string customerName, string customerMobile, string bikeName, string areaName, string cityName)
        {
            Bikewale.Common.SMSTypes obj = new Bikewale.Common.SMSTypes();
            obj.NewBikePriceQuoteSMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, HttpContext.Current.Request.ServerVariables["URL"].ToString());
        }

        public static void SMSToCustomer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            Bikewale.Common.SMSTypes obj = new Bikewale.Common.SMSTypes();
            obj.NewBikePriceQuoteSMSToCustomer(customerMobile, customerName, BikeName, dealerName, dealerContactNo, dealerAddress, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingAmount, insuranceAmount, hasBumperDealerOffer);
        }

        public static void BookingSMSToCustomer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string bookingRefNum, uint insuranceAmount = 0)
        {
            Bikewale.Common.SMSTypes obj = new Bikewale.Common.SMSTypes();
            obj.BikeBookingSMSToCustomer(customerMobile, customerName, BikeName, dealerName, dealerContactNo, dealerAddress, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingRefNum, insuranceAmount);
        }

        public static void BookingSMSToDealer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string bookingRefNum, UInt32 bookingAmt, uint insuranceAmount = 0)
        {
            Bikewale.Common.SMSTypes obj = new Bikewale.Common.SMSTypes();
            obj.BikeBookingSMSToDealer(dealerContactNo, customerName, dealerName, customerMobile, BikeName, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingAmt, bookingRefNum, insuranceAmount);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22 Dec 2014
        /// Summary : To send prebooking bike email to customer
        /// Modified By : Lucky Rathore on 11 May 2016.
        /// Summary : price List, versionName, makeModelName, dealer mail, working hour, lat long added.
        /// </summary>
        /// <param name="customerEmail"></param>
        /// <param name="customerName"></param>
        /// <param name="offerList"></param>
        /// <param name="bookingReferenceNo"></param>
        /// <param name="preBookingAmount"></param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerAddress"></param>
        /// <param name="dealerMobile"></param>
        public static void BookingEmailToCustomer(string customerEmail, string customerName, List<PQ_Price> priceList, List<OfferEntity> offerList, string bookingReferenceNo, uint totalAmount, uint preBookingAmount, string makeModelName, string version, string color, string img, string dealerName, string dealerAddress, string dealerMobile, string dealerEmailId, string dealerWorkingTime, double dealerLatitude, double dealerLongitude)
        {
            ComposeEmailBase objEmail = new PreBookingConfirmationToCustomer(customerName, priceList, offerList, bookingReferenceNo, totalAmount, preBookingAmount, makeModelName, version, color, img, dealerName, dealerAddress, dealerMobile, dealerEmailId, dealerWorkingTime, dealerLatitude, dealerLongitude);
            objEmail.Send(customerEmail, "Congratulations on pre-booking the " + makeModelName, "");
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Dec 2014
        /// Summary : to send prebooking email to dealer
        /// Modified By : Vivek Gupta on 11-5-2016
        /// Desc : versionName added in BookingEmailToDealer
        /// </summary>
        /// <param name="dealerEmail"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerArea"></param>
        /// <param name="customerEmail"></param>
        /// <param name="totalPrice"></param>
        /// <param name="bookingAmount"></param>
        /// <param name="balanceAmount"></param>
        /// <param name="priceList"></param>
        /// <param name="bookingReferenceNo"></param>
        /// <param name="bikeName"></param>
        /// <param name="bikeColor"></param>
        /// <param name="dealerName"></param>
        /// <param name="offerList"></param>
        public static void BookingEmailToDealer(string dealerEmail, string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount, uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, string imagePath, List<OfferEntity> offerList, string versionName)
        {
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');
                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new PreBookingConfirmationMailToDealer(customerName, customerMobile, customerArea, customerEmail, totalPrice, bookingAmount, balanceAmount, priceList, bookingReferenceNo, bikeName, bikeColor, dealerName, offerList, imagePath, versionName);
                    objEmail.Send(email, "BW Pre-Booking: " + customerName + " paid Rs. " + bookingAmount + " for " + bikeName + " - " + bikeColor, "");
                }
            }
        }

        /// <summary>
        /// Author              : Sumit Kate
        /// Date                : 04/06/2015
        /// Pivotal Tracker Id  :   96159278
        /// Modified By : Vivek Gupta on 11-5-2016
        /// Desc : versionName added in BookingEmailToDealerv
        /// This is overloaded method. It keeps BikeWale representative in CC.
        /// </summary>
        /// <param name="dealerEmail">Dealers Email ids comma separated</param>
        /// <param name="bikewaleEmail">BikeWale representative email ids comma separated.</param>
        /// <param name="customerName">Customer Name</param>
        /// <param name="customerMobile">Customer Mobile Number</param>
        /// <param name="customerArea">Customer Area</param>
        /// <param name="customerEmail">Customer Email Id</param>
        /// <param name="totalPrice">Total Price</param>
        /// <param name="bookingAmount">Booking amount</param>
        /// <param name="balanceAmount">Balance amount</param>
        /// <param name="priceList">Price List</param>
        /// <param name="bookingReferenceNo">Booking Reference number</param>
        /// <param name="bikeName">Bike name</param>
        /// <param name="bikeColor">Bike Color</param>
        /// <param name="dealerName">Dealer Name</param>
        /// <param name="offerList">Offer Lists</param>
        public static void BookingEmailToDealer(string dealerEmail, string bikewaleEmail, string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount, uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, List<OfferEntity> offerList, string imagePath, string versionName, uint insuranceAmount = 0)
        {
            string[] arrBikeWaleEmail = null;
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');
                arrBikeWaleEmail = bikewaleEmail.Split(',');
                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new PreBookingConfirmationMailToDealer(customerName, customerMobile, customerArea, customerEmail, totalPrice, bookingAmount, balanceAmount, priceList, bookingReferenceNo, bikeName, bikeColor, dealerName, offerList, imagePath, versionName, insuranceAmount);
                    objEmail.Send(email, "BW Pre-Booking: " + customerName + " paid Rs. " + bookingAmount + " for " + bikeName + " - " + bikeColor, "", null, arrBikeWaleEmail);
                }
            }
        }
    }
}