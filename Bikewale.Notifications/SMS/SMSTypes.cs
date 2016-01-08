using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications.NotificationDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Notifications
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class SMSTypes
    {
        public void RegistrationSMS(string name, string password, string number, string eMail, string pageUrl)
        {
            string message = String.Empty;
            try
            {
                message = String.Format("Dear {0}, Thank you for your registration at BikeWale. Your password is {1} and your loginid is the eMail id you provided.", name, password);

                EnumSMSServiceType esms = EnumSMSServiceType.CustomerRegistration;
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.RegistrationSMS : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.RegistrationSMS");
                objErr.SendMail();
            }

        }

        // FUNCTION TO SEND ADDRESS OF THE DEALER TO THE REQUESTED CUSTOMER FROM DEALER SHOWROOM
        public void SMSDealerAddress(string number, string address, string pageUrl)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.DealerAddressRequest;

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, address, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.SMSDealerAddress : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.SMSDealerAddress");
                objErr.SendMail();
            }
        }//

        // FUNCTION TO SEND ADDRESS OF THE DEALER TO THE REQUESTED CUSTOMER FROM DEALER SHOWROOM
        public void SMSNewBikeQuote(string number, string cityName, string bike, string onRoad, string exShowRoom,
                                        string rto, string insurance, string pageUrl)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikeQuote;

                string message = "";

                message = "Price details of new " + bike + " in " + cityName + ":\n"
                        + " Ex-Showroom-" + exShowRoom + ",\n"
                        + " Insurance-" + insurance + ",\n"
                        + " RTO-" + rto + ",\n"
                        + " OnRoad-" + onRoad;

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.SMSNewBikeQuote : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.SMSNewBikeQuote");
                objErr.SendMail();
            }
        }//

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 22 Dec 2015 
        /// Summary : To push sms in priority queue
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="pageUrl"></param>
        public void SMSMobileVerification(string number, string name, string code, string pageUrl)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.MobileVerification;

                string message = "";

                message = code + " is the OTP for verifying your mobile number at BikeWale. This is a one time verification process.";

                SMSCommon sc = new SMSCommon();
                sc.ProcessPrioritySMS(number, message, esms, pageUrl, true);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.SMSNewBikeQuote : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.SMSNewBikeQuote");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 9 Nov 2014
        /// Summary : To send sms to dealer for new bike price quote
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="pageUrl"></param>
        public void NewBikePriceQuoteSMSToDealer(string dealerMobileNo, string customerName, string customerMobile, string BikeName, string areaName, string cityName, string pageUrl)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToDealer;

                string message = NewBikePQDealerSMSTemplate(customerName, customerMobile, BikeName, areaName, cityName);

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(dealerMobileNo, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.NewBikePriceQuoteSMSToDealer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.NewBikePriceQuoteSMSToDealer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 27 Nov 2014
        /// Summary : To send sms to customer for new bike price quote
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="BikeName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerContactNo"></param>
        /// <param name="dealerAddress"></param>
        /// <param name="pageUrl"></param>
        public void NewBikePriceQuoteSMSToCustomer(PQ_DealerDetailEntity dealerEntity, string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string pageUrl, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            try
            {
                // To check if user has accepted offer with respect to Flipkart vouchers
                bool isFlipkartOffer = false;
                bool isAccessories = false;

                if (dealerEntity.objOffers != null && dealerEntity.objOffers.Count > 0)
                {
                    foreach (var offer in dealerEntity.objOffers)
                    {
                        if (offer.OfferText.ToLower().Contains("flipkart"))
                        {
                            isFlipkartOffer = true;
                            break;
                        }
                        else if (offer.OfferText.ToLower().Contains("accessories"))
                        {
                            isAccessories = true;
                            break;
                        }
                    }
                }
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToCustomer;

                string message = NewBikePQCustomerSMSTemplate(BikeName, dealerName, dealerContactNo, dealerAddress, bookingAmount, insuranceAmount, hasBumperDealerOffer, isFlipkartOffer, isAccessories);
                
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(customerMobile, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.NewBikePriceQuoteSMSToCustomer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.NewBikePriceQuoteSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 19 Dec 2014
        /// Summary    : Method to send sms to customer after bike is booked successfully.
        /// </summary>
        /// <param name="customerMobile"></param>
        /// <param name="customerName"></param>
        /// <param name="BikeName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerContactNo"></param>
        /// <param name="dealerAddress"></param>
        /// <param name="pageUrl"></param>
        public void BikeBookingSMSToCustomer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string pageUrl, string bookingRefNum, uint insuranceAmount = 0)
        {
            bool isOfferAvailable = false;
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.BikeBookedSMSToCustomer;

                string message = "";
                isOfferAvailable = Convert.ToBoolean(ConfigurationManager.AppSettings["isOfferAvailable"]);

                if (insuranceAmount == 0)
                {
                    message = "Congratulations! You have booked " + BikeName + ", Reference No " + bookingRefNum + ". Contact " + dealerName + ", " + dealerContactNo + ", " + dealerAddress;
                }
                else
                {
                    message = String.Format("You have pre-booked {0}, No {1}. Contact {2}, {3}, {4}. Avail your Free Insurance at the dealership.", BikeName, bookingRefNum, dealerName, dealerContactNo, dealerAddress);
                }

                SMSCommon sc = new SMSCommon();
                if (isOfferAvailable && insuranceAmount == 0)
                {
                    message += String.Format(". Avail your BIKEWALE OFFER at {0}, after you have purchased your bike.", ConfigurationManager.AppSettings["BkgConfirmOfferPageShortUrl"]);
                }
                sc.ProcessSMS(customerMobile, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.BikeBookingSMSToCustomer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.BikeBookingSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 19 Dec 2014
        /// Summary    : Method to send sms to dealer after user booked bike successfully.
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="dealerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="pageUrl"></param>
        /// <param name="bookingAmt"></param>
        /// <param name="bookingRefNum"></param>
        public void BikeBookingSMSToDealer(string dealerMobileNo, string customerName, string dealerName, string customerMobile, string BikeName, string pageUrl, UInt32 bookingAmt, string bookingRefNum, uint insuranceAmount = 0)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.BikeBookedSMSToDealer;

                string message = "";
                if (insuranceAmount == 0)
                {
                    message = "BW Pre-Booking: " + customerName + " paid Rs " + bookingAmt + " for " + BikeName + ", No " + bookingRefNum + ". Call customer at " + customerMobile + " to schedule visit.";
                }
                else
                {
                    message = String.Format("BW Pre-Booking: {0} paid Rs {1} for {2}, No {3}. Call customer at {4} to schedule visit. Customer is eligible for Free Insurance Offer.", customerName, bookingAmt, BikeName, bookingRefNum, customerMobile);
                }

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(dealerMobileNo, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.NewBikePriceQuoteSMSToDealer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.NewBikePriceQuoteSMSToDealer");
                objErr.SendMail();
            }
        }

        public void ClaimedOfferSMSToCustomer(string customerMobile, string pageUrl)
        {
            try 
            {
                EnumSMSServiceType esms = EnumSMSServiceType.ClaimedOffer;
                string message = "Thank you for providing your bike details. After verifying, we will ship the gifts to you within 30 days. Write to contact@bikewale.com in case of any concerns.";
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(customerMobile, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Notifications.ClaimedOfferSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To send sms to dealer for new bike price quote
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="pageUrl"></param>
        public void SaveNewBikePriceQuoteSMSToDealer(uint pqId, string dealerMobileNo, string customerName, string customerMobile, string BikeName, string areaName, string cityName, string pageUrl)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToDealer;

                string message = NewBikePQDealerSMSTemplate(customerName, customerMobile, BikeName, areaName, cityName);

                SavePQNotification obj = new SavePQNotification();
                obj.SaveDealerPQSMSTemplate(pqId,message, (int)esms, dealerMobileNo, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.NewBikePriceQuoteSMSToDealer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.NewBikePriceQuoteSMSToDealer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To Save sms to customer for new bike price quote
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="BikeName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerContactNo"></param>
        /// <param name="dealerAddress"></param>
        /// <param name="pageUrl"></param>
        public void SaveNewBikePriceQuoteSMSToCustomer(uint pqId, PQ_DealerDetailEntity dealerEntity, string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string pageUrl, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            try
            {
                // To check if user has accepted offer with respect to Flipkart vouchers
                bool isFlipkartOffer = false;
                bool isAccessories = false;
                
                if (dealerEntity.objOffers != null && dealerEntity.objOffers.Count > 0)
                {
                    foreach (var offer in dealerEntity.objOffers)
                    {
                        if (offer.OfferText.ToLower().Contains("flipkart"))
                        {
                            isFlipkartOffer = true;
                            break;
                        }
                        else if (offer.OfferText.ToLower().Contains("accessories"))
                        {
                            isAccessories = true;
                            break;
                       }
                    }
                }
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToCustomer;

                string message = NewBikePQCustomerSMSTemplate(BikeName, dealerName, dealerContactNo, dealerAddress, bookingAmount, insuranceAmount, hasBumperDealerOffer, isFlipkartOffer, isAccessories);

                SavePQNotification obj = new SavePQNotification();
                obj.SaveCustomerPQSMSTemplate(pqId, message, (int)esms, customerMobile, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Notifications.SaveNewBikePriceQuoteSMSToCustomer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Notifications.NewBikePriceQuoteSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : to get new bike price quote customer sms template
        /// </summary>
        /// <param name="BikeName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerContactNo"></param>
        /// <param name="dealerAddress"></param>
        /// <param name="bookingAmount"></param>
        /// <param name="insuranceAmount"></param>
        /// <param name="hasBumperDealerOffer"></param>
        /// <param name="isFlipkartOffer"></param>
        /// <returns></returns>
        private static string NewBikePQCustomerSMSTemplate(string BikeName, string dealerName, string dealerContactNo, string dealerAddress, uint bookingAmount, uint insuranceAmount, bool hasBumperDealerOffer, bool isFlipkartOffer, bool isAccessories)
        {
            string message = "";
            //message = "Dear " + customerName + ", Thank you for showing interest in " + BikeName + ". Dealer details: " + dealerName + ", " + dealerContactNo + ", " + dealerAddress;
            if (!hasBumperDealerOffer)
            {
                if (insuranceAmount == 0)
                {
                    if (isFlipkartOffer)
                    {
                        //message = String.Format("Pay Rs. {0} on BikeWale to book your bike, pay balance amount at {1} {2} ({3}), and claim Free Rs. 1,000 Flipkart vouchers & 1-year RSA from BikeWale.", bookingAmount, dealerName, dealerAddress, dealerContactNo);
                        message = String.Format("Pay Rs. {0} on BikeWale to book your bike, pay balance amount at {1} {2} ({3}), and claim Free Rs. 1,000 Flipkart vouchers.", bookingAmount, dealerName, dealerAddress, dealerContactNo);
                    }
                    else if(isAccessories)
                    {
                        message = String.Format("Pay Rs. {0} on BikeWale to book your bike, pay balance amount at {1} {2} ({3}), and claim Free Accessories at the dealership.", bookingAmount, dealerName, dealerAddress, dealerContactNo);
                    }
                    else
                    {
                        //message = String.Format("Avail your FREE Vega Helmet %26 1-year RSA from BikeWale on purchase of {0} from {1}({2}) Dealer Address: {3}.", BikeName, dealerName, dealerContactNo, dealerAddress);
                        //message = String.Format("Pay Rs. {1} to book your {0} at BikeWale to get a helmet worth Rs. 1000 and one year RSA absolutely FREE!", BikeName, bookingAmount);
                        message = String.Format("Pay Rs. {0} on BikeWale to book your bike, pay balance amount at {1} {2} ({3}), and claim Free Helmet %26 1-year RSA from BikeWale.", bookingAmount, dealerName, dealerAddress, dealerContactNo);
                    }
                }
                else
                {
                    message = String.Format("Pay Rs. {3} to book your {0} online at BikeWale %26 get 100%25 discount on Insurance at {1}({2})", BikeName, dealerName, dealerContactNo, bookingAmount);
                }
            }
            else
            {
                message = String.Format("Pay Rs. {0} to book your {1} at BikeWale to get free insurance, free accessories worth Rs. 3,000 and discount on bike worth Rs. 1,000 at the dealership!", bookingAmount, BikeName);
            }
            return message;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To get new bike price quote dealer template
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private static string NewBikePQDealerSMSTemplate(string customerName, string customerMobile, string BikeName, string areaName, string cityName)
        {
            string message = "";

            message = "BikeWale purchase enquiry: Please call " + customerName + ", " + areaName + ", " + cityName + " at " + customerMobile + " for " + BikeName + " and schedule customer visit.";
            return message;
        }

        public void SaveNewBikePriceQuoteSMSToCustomer(uint pqId, string message, string customerMobile, string requestUrl)
        {
            try                
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToCustomer;
                SavePQNotification obj = new SavePQNotification();
                obj.SaveCustomerPQSMSTemplate(pqId, message, (int)esms, customerMobile, requestUrl);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.SMSTypes.SaveNewBikePriceQuoteSMSToCustomer");
                objErr.SendMail();
            }
        }

    }   //End of class
}   //End of namespace
