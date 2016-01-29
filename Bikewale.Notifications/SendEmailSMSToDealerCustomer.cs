﻿using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications.MailTemplates;
using Bikewale.Notifications.NotificationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created By : Sadhana Upadhyay
    /// </summary>
    public class SendEmailSMSToDealerCustomer
    {
        public static void SendEmailToDealer(string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList,string imagePath, uint insuranceAmount = 0)
        {
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');

                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new NewBikePriceQuoteMailToDealerTemplate(makeName, modelName, dealerName, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, DateTime.Now, imagePath, insuranceAmount);
                    objEmail.Send(email, "BikeWale Purchase Inquiry - " + makeName + " " + modelName + " " + versionName, customerEmail);
                }
            }
        }

        public static void SendEmailToCustomer(string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, uint isInsuranceFree = 0)
        {
            ComposeEmailBase objEmail = new NewBikePriceQuoteToCustomerTemplate(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, "", address, customerName, DateTime.Now, priceList, offerList, pinCode, stateName, cityName, totalPrice, isInsuranceFree);
            objEmail.Send(customerEmail, "Your Dealer Price Certificate - " + bikeName, dealerEmail);
        }

        public static void SMSToDealer(string dealerMobile, string customerName, string customerMobile, string bikeName, string areaName, string cityName)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            obj.NewBikePriceQuoteSMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, HttpContext.Current.Request.ServerVariables["URL"].ToString());
        }

        public static void SMSToCustomer(PQ_DealerDetailEntity dealerEntity, string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            obj.NewBikePriceQuoteSMSToCustomer(dealerEntity, customerMobile, customerName, BikeName, dealerName, dealerContactNo, dealerAddress, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingAmount, insuranceAmount, hasBumperDealerOffer);
        }

        public static void BookingSMSToCustomer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string bookingRefNum, uint insuranceAmount = 0)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            obj.BikeBookingSMSToCustomer(customerMobile, customerName, BikeName, dealerName, dealerContactNo, dealerAddress, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingRefNum, insuranceAmount);
        }

        public static void BookingSMSToDealer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string bookingRefNum, UInt32 bookingAmt, uint insuranceAmount = 0)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            obj.BikeBookingSMSToDealer(dealerContactNo, customerName, dealerName, customerMobile, BikeName, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingAmt, bookingRefNum, insuranceAmount);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22 Dec 2014
        /// Summary : To send prebooking bike email to customer
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
        public static void BookingEmailToCustomer(string customerEmail, string customerName, List<OfferEntity> offerList, string bookingReferenceNo, uint preBookingAmount, string bikeName, string makeName, string modelName, string dealerName, string dealerAddress, string dealerMobile, uint insuranceAmount = 0)
        {
            ComposeEmailBase objEmail = new PreBookingConfirmationToCustomer(customerName, offerList, bookingReferenceNo, preBookingAmount, bikeName, makeName, modelName, dealerName, dealerAddress, dealerMobile, DateTime.Now, insuranceAmount);
            objEmail.Send(customerEmail, "Congratulations on pre-booking the " + bikeName, "");
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Dec 2014
        /// Summary : to send prebooking email to dealer
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
        public static void BookingEmailToDealer(string dealerEmail, string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount, uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, string imagePath, List<OfferEntity> offerList)
        {
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');
                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new PreBookingConfirmationMailToDealer(customerName, customerMobile, customerArea, customerEmail, totalPrice, bookingAmount, balanceAmount, priceList, bookingReferenceNo, bikeName, bikeColor, dealerName,  offerList, imagePath);
                    objEmail.Send(email, "BW Pre-Booking: " + customerName + " paid Rs. " + bookingAmount + " for " + bikeName + " - " + bikeColor, "");
                }
            }
        }

        /// <summary>
        /// Author              : Sumit Kate
        /// Date                : 04/06/2015
        /// Pivotal Tracker Id  :   96159278
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
        public static void BookingEmailToDealer(string dealerEmail, string bikewaleEmail, string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount, uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, List<OfferEntity> offerList, string imagePath, uint insuranceAmount = 0)
        {
            string[] arrBikeWaleEmail = null;
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');
                arrBikeWaleEmail = bikewaleEmail.Split(',');
                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new PreBookingConfirmationMailToDealer(customerName, customerMobile, customerArea, customerEmail, totalPrice, bookingAmount, balanceAmount, priceList, bookingReferenceNo, bikeName, bikeColor, dealerName, offerList, imagePath, insuranceAmount);
                    objEmail.Send(email, "BW Pre-Booking: " + customerName + " paid Rs. " + bookingAmount + " for " + bikeName + " - " + bikeColor, "", null, arrBikeWaleEmail);
                }
            }
        }


        #region Save sms and email information of the customer and dealer after generating the leads

        public static void SaveEmailToDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList,  string imagePath, uint insuranceAmount = 0)
        {
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                string[] arrDealerEmail = dealerEmail.Split(',');

                foreach (string email in arrDealerEmail)
                {
                    ComposeEmailBase objEmail = new NewBikePriceQuoteMailToDealerTemplate(makeName, modelName, dealerName, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, DateTime.Now, imagePath, insuranceAmount);

                    string emailBody = objEmail.ComposeBody();

                    // Save the template into database and other parameters

                    //objEmail.Send(email, "BikeWale Purchase Inquiry - " + makeName + " " + modelName + " " + versionName, customerEmail);
                    SavePQNotification obj = new SavePQNotification();
                    obj.SaveDealerPQEmailTemplate(pqId, emailBody, "BikeWale Purchase Inquiry - " + makeName + " " + modelName + " " + versionName, customerEmail);
                }
            }
        }

        public static void SaveEmailToCustomer(uint pqId, string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, uint isInsuranceFree = 0)
        {
            ComposeEmailBase objEmail = new NewBikePriceQuoteToCustomerTemplate(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, "", address, customerName, DateTime.Now, priceList, offerList, pinCode, stateName, cityName, totalPrice, isInsuranceFree);

            // Save the template into database and other parameters

            string emailBody = objEmail.ComposeBody();

            SavePQNotification obj = new SavePQNotification();
            obj.SaveCustomerPQEmailTemplate(pqId, emailBody, "Your Dealer Price Certificate - " + bikeName, dealerEmail.Split(',')[0]);

            //objEmail.Send(customerEmail, "Your Dealer Price Certificate - " + bikeName, dealerEmail);
        }

        public static void SaveSMSToDealer(uint pqId, string dealerMobile, string customerName, string customerMobile, string bikeName, string areaName, string cityName)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            obj.SaveNewBikePriceQuoteSMSToDealer(pqId, dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, HttpContext.Current.Request.ServerVariables["URL"].ToString());

            // Save the template into database and other parameters

        }

        public static void SaveSMSToCustomer(uint pqId, PQ_DealerDetailEntity dealerEntity, string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            Bikewale.Notifications.SMSTypes obj = new Bikewale.Notifications.SMSTypes();
            // Save the template into database and other parameters
            obj.SaveNewBikePriceQuoteSMSToCustomer(pqId, dealerEntity, customerMobile, customerName, BikeName, dealerName, dealerContactNo, dealerAddress, HttpContext.Current.Request.ServerVariables["URL"].ToString(), bookingAmount, insuranceAmount, hasBumperDealerOffer);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 Jan 2016
        /// Description :   Saves the Customer Lead SMS
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <param name="objDPQSmsEntity">DPQ SMS Entity</param>
        /// <param name="DPQType"></param>
        /// <param name="requestUrl"></param>
        public static void SaveSMSToCustomer(uint pqId, string requestUrl, DPQSmsEntity objDPQSmsEntity, DPQTypes DPQType)
        {
            string message = String.Empty;

            try
            {
                SMSTypes obj = new SMSTypes();
                switch (DPQType)
                {
                    case DPQTypes.NoOfferNoBooking:
                        message = String.Format("{0},{1} ({2}) will call you regarding your bike inquiry on BikeWale. For more details, visit {3}", objDPQSmsEntity.DealerName, objDPQSmsEntity.Locality, objDPQSmsEntity.DealerMobile, objDPQSmsEntity.LandingPageShortUrl);
                        break;
                    case DPQTypes.NoOfferOnlineBooking:
                        message = String.Format("You can now book {0} by just paying Rs. {1} at your convenience. This amount will be adjusted against the total payment. For more details, visit {2}", objDPQSmsEntity.BikeName, objDPQSmsEntity.BookingAmount, objDPQSmsEntity.LandingPageShortUrl);
                        break;
                    case DPQTypes.OfferNoBooking:
                        message = String.Format("We are running exciting offers on purchase of {0} from {1},{2}. Hurry! Offer valid till stock lasts. For more details, visit {3}", objDPQSmsEntity.BikeName, objDPQSmsEntity.DealerName, objDPQSmsEntity.Locality, objDPQSmsEntity.LandingPageShortUrl);
                        break;
                    case DPQTypes.OfferAndBooking:
                        message = String.Format("We are running exciting offers on online booking of {0} at BikeWale. Hurry! Offer valid till stock lasts. For more details, visit {1}", objDPQSmsEntity.BikeName, objDPQSmsEntity.LandingPageShortUrl);
                        break;
                    case DPQTypes.AndroidAppNoOfferNoBooking:
                    case DPQTypes.AndroidAppOfferNoBooking:
                        message = String.Format("Hi {0}, thanks for your interest on BikeWale. {1},{2} ({3}) will call you regarding your bike inquiry.", objDPQSmsEntity.CustomerName, objDPQSmsEntity.DealerName, objDPQSmsEntity.Locality, objDPQSmsEntity.DealerMobile);
                        break;
                }
                if (objDPQSmsEntity != null && !String.IsNullOrEmpty(objDPQSmsEntity.CustomerMobile) && !String.IsNullOrEmpty(message) && pqId > 0)
                {
                    obj.SaveNewBikePriceQuoteSMSToCustomer(pqId, message, objDPQSmsEntity.CustomerMobile, requestUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.SendEmailSMSToDealerCustomer.SaveSMSToCustomer");
                objErr.SendMail();
            }
        }

        #endregion
    }
}
