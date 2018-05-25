using Bikewale.DTO.MobileVerification;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.MobileVerification;
using Bikewale.Notifications.NotificationDAL;
using Bikewale.Utility;
using System;
using System.Configuration;
using System.Text;
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
                ErrorClass.LogError(err, "Notifications.RegistrationSMS");

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
                ErrorClass.LogError(err, "Notifications.SMSDealerAddress");

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
                ErrorClass.LogError(err, "Notifications.SMSNewBikeQuote");

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

                string message = string.Empty;

                message = code + " is the OTP for verifying your mobile number at BikeWale. This is a one time verification process.";

                SMSCommon sc = new SMSCommon();
                sc.ProcessPrioritySMS(number, message, esms, pageUrl, true);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Notifications.SMSNewBikeQuote");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 9 Nov 2014
        /// Summary : To send sms to dealer for new bike price quote
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added. 
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="pageUrl"></param>
        public void NewBikePriceQuoteSMSToDealer(string dealerMobileNo, string customerName, string customerMobile, string BikeName, string areaName, string cityName, string pageUrl, string dealerArea)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToDealer;

                string message = NewBikePQDealerSMSTemplate(customerName, customerMobile, BikeName, areaName, cityName, dealerArea);

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(dealerMobileNo, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Notifications.NewBikePriceQuoteSMSToDealer");

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
                ErrorClass.LogError(err, "Notifications.NewBikePriceQuoteSMSToCustomer");

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
                ErrorClass.LogError(err, "Notifications.BikeBookingSMSToCustomer");

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
                ErrorClass.LogError(err, "Notifications.NewBikePriceQuoteSMSToDealer");

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
                ErrorClass.LogError(err, "Notifications.ClaimedOfferSMSToCustomer");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To send sms to dealer for new bike price quote
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added. 
        /// </summary>
        /// <param name="dealerMobileNo"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="pageUrl"></param>
        public void SaveNewBikePriceQuoteSMSToDealer(uint pqId, string dealerMobileNo, string customerName, string customerMobile, string BikeName, string areaName, string cityName, string pageUrl, string dealerArea)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToDealer;

                string message = NewBikePQDealerSMSTemplate(customerName, customerMobile, BikeName, areaName, cityName, dealerArea);

                SavePQNotification obj = new SavePQNotification();
                obj.SaveDealerPQSMSTemplate(pqId, message, (int)esms, dealerMobileNo, pageUrl);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Notifications.NewBikePriceQuoteSMSToDealer");

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
                ErrorClass.LogError(err, "Notifications.NewBikePriceQuoteSMSToCustomer");

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
                    else if (isAccessories)
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
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added and SMS text changed.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="BikeName"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private static string NewBikePQDealerSMSTemplate(string customerName, string customerMobile, string BikeName, string areaName, string cityName, string dealerArea)
        {
            string message = "";
            if (!string.IsNullOrEmpty(areaName))
            {
                areaName = ", " + areaName;
            }
            message = string.Format("BikeWale purchase enquiry for {0} showroom: Please call {1}{2}, {3} at {4} for {5} and schedule customer visit.", dealerArea, customerName, areaName, cityName, customerMobile, BikeName);
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
                ErrorClass.LogError(ex, "Bikewale.Notifications.SMSTypes.SaveNewBikePriceQuoteSMSToCustomer");

            }
        }

        /// <summary>
        /// Modified By : Sangram Nandkhile on 22 Jan 2016 
        /// Summary : To push OTP to booking cancellation SMS in priority queue
        /// </summary>
        public void SMSMobileVerification(string number, string otp, string pageUrl)
        {
            try
            {
                EnumSMSServiceType smsEnum = EnumSMSServiceType.BookingCancellationOTP;
                string message = string.Empty;
                message = string.Format("{0} is the OTP for initiating your cancellation request on BikeWale. Please enter the OTP on BikeWale site for initiating cancellation.", otp);

                SMSCommon sc = new SMSCommon();
                sc.ProcessPrioritySMS(number, message, smsEnum, pageUrl, true);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Notifications.SMSBikeBookingCancellation");

            }
        }
        /// <summary>
        /// Created By  : Subodh Jain on 25-Nov-2016
        /// Description : Send SMS to customer For Photo Upload.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerNumber"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="profileId"></param>
        public void SMSForPhotoUploadTwoDays(string customerName, string customerNumber, string make, string model, string profileId, string editUrl)
        {
            try
            {
                EnumSMSServiceType smsEnum = EnumSMSServiceType.SMSForPhotoUploadTwoDays;
                string message = string.Empty;

                message = string.Format("Add high-quality images to your {0} {1} bike Ad. Ads with photos get 50% more responses. Click here to add photos - {2}. Team BikeWale", make, model, editUrl);

                SMSCommon sc = new SMSCommon();
                sc.ProcessPrioritySMS(customerNumber, message, smsEnum, "SMSTypes.SMSForPhotoUploadTwoDays", true);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "SMSTypes.SMSForPhotoUploadTwoDays");

            }

        }


        public void BookingCancallationSMSToUser(string number, string customerName, string pageUrl)
        {
            try
            {
                EnumSMSServiceType smsEnum = EnumSMSServiceType.BookingCancellationToCustomer;
                string message = string.Empty;
                message = string.Format("Hi {0}, we have received your request for cancellation. We will initiate the refund process after confirming your details with the dealership.", customerName);

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, smsEnum, pageUrl, true);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Notifications.SMSBikeBookingCancellation");

            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 23 Sep 2016
        /// Description :   Sends the Used bike purchase inquiry sms
        /// </summary>
        /// <param name="smsType"></param>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <param name="pageurl"></param>
        public void UsedPurchaseInquirySMS(EnumSMSServiceType smsType, string number, string message, string pageurl)
        {
            try
            {
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.UsedPurchaseInquirySMSToSeller({0},{1},{2})", number, message, pageurl));

            }
        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 28 Oct 2016
        /// Description : Send SMS to seller on approval of used bike listing
        /// </summary>
        /// <param name="smsType"></param>
        /// <param name="number"></param>
        /// <param name="profileId"></param>
        /// <param name="customerName"></param>
        /// <param name="pageurl"></param>
        public void ApprovalUsedSellListingSMS(EnumSMSServiceType smsType, string number, string profileId, string customerName, string pageurl)
        {
            string message = String.Format("Hi {0}, your bike listing (profile id - {1}) has been verified and approved on BikeWale. We wish you good luck with your listing.", customerName, profileId.ToUpper());
            try
            {
                SMSCommon sc = new SMSCommon();

                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.ApprovalUsedSellListingSMSToSeller({0},{1},{2},{3})", number, message, pageurl, profileId));

            }
        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 28 Oct 2016
        /// Description : Send SMS to seller on rejection of used bike listing
        /// </summary>
        public void RejectionUsedSellListingSMS(EnumSMSServiceType smsType, string number, string profileId, string pageurl)
        {
            string message = String.Format("Your bike listing (profile id - {0}) has not been approved on BikeWale. Listing price or photos could be an issue. Please re-submit it with correct info.", profileId.ToUpper());
            try
            {
                SMSCommon sc = new SMSCommon();

                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.RejectionUsedSellListingSMS({0},{1},{2},{3})", number, message, pageurl, profileId));

            }
        }


        public void UsedSellSuccessfulListingSMS(EnumSMSServiceType smsType, string number, string profileid, string pageurl)
        {
            string message = String.Format("Congratulations! You have successfully posted a bike listing (profile id -{0}) on BikeWale. It will be verified by us before it is made available to buyers.", profileid.ToUpper());
            try
            {
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.UsedSellSuccessfulListingSMS({0},{1},{2})", number, message, pageurl));

            }
        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 9 Nov 2016
        /// Description : Send SMS to seller on approval of changes in used bike listing
        /// </summary>
        public void ApprovalEditedUsedSellListingSMS(EnumSMSServiceType smsType, string number, string profileId, string customerName, string pageurl)
        {
            string message = String.Format("Hi {0}, the changes in your bike listing(profile id - {1}) have been verified and approved on BikeWale. We wish you good luck with your listing.", customerName, profileId.ToUpper());
            try
            {
                SMSCommon sc = new SMSCommon();

                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.ApprovalEditedUsedSellListingSMS({0},{1},{2},{3})", number, message, pageurl, profileId));

            }
        }

        /// <summary>
        /// Created By  : Aditi Srivastava on 14 Nov 2016
        /// Description : Send SMS to seller on rejection of changes in used bike listing
        /// </summary>
        public void RejectionEditedUsedSellListingSMS(EnumSMSServiceType smsType, string number, string profileId, string customerName, string pageurl)
        {
            string message = String.Format("Hi {0}, the changes to your bike listing(profile id - {1}) have been discarded on BikeWale. Listing price or photos could be an issue. Please re-submit it with correct info.", customerName, profileId.ToUpper());
            try
            {
                SMSCommon sc = new SMSCommon();

                sc.ProcessSMS(number, message, smsType, pageurl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.RejectionEditedUsedSellListingSMS({0},{1},{2},{3})", number, message, pageurl, profileId));

            }
        }

        /// <summary>
        /// Created By  : Sajal Gupta on 16-11-2016
        /// Description : Send SMS to customer on requesting service center details.
        /// Modiefier   : Kartik Rathod on 30 apl 2018,  Change formatting for dealer address,make static method
        /// </summary>
        public static void ServiceCenterDetailsSMS(SMSData objSMSData, string mobileNumber, string pageUrl)
        {
            try
            {
                UrlShortner objUrlShortner = new UrlShortner();

                var shortUrlObj = objUrlShortner.GetShortUrl(string.Format("https://maps.google.com/?saddr=&daddr={0},{1}", objSMSData.Latitude, objSMSData.Longitude));
                string shortUrl = shortUrlObj != null ? shortUrlObj.ShortUrl : "";
                StringBuilder messageBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(objSMSData.Name))
                {
                    messageBuilder.Append(objSMSData.Name);
                    messageBuilder.Append(",");
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!String.IsNullOrEmpty(objSMSData.Address))
                {
                    messageBuilder.Append(objSMSData.Address);
                    messageBuilder.Append(",");
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!String.IsNullOrEmpty(objSMSData.Phone))
                {
                    messageBuilder.Append("+91-");
                    messageBuilder.Append(objSMSData.Phone);
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(shortUrl))
                {
                    messageBuilder.Append("Get Directions-");
                    messageBuilder.Append(shortUrl);
                    messageBuilder.Append(Environment.NewLine);
                }
                messageBuilder.Append("Thanks for visiting BikeWale.");
                string message = messageBuilder.ToString();

                EnumSMSServiceType esms = EnumSMSServiceType.ServiceCenterDetailsSMSToCustomer;
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(mobileNumber, message, esms, pageUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.ServiceCenterDetailsSMS({0},{1},{2},{3},{4},{5})", mobileNumber, objSMSData.Name,
                    objSMSData.Address, objSMSData.Phone, pageUrl));

            }
        }


        /// <summary>
        /// Created By  : Sajal Gupta on 23-11-2016
        /// Description : Send SMS to seller for notifying expiry of listing.
        /// </summary>
        public void ExpiringListingReminderSMS(string number, string pageUrl, EnumSMSServiceType esms, string message)
        {
            try
            {
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, esms, pageUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.ExpiringListingReminderSMS({0},{1},{2},{3})", number, pageUrl, esms, message));

            }
        }


        /// <summary>
        /// Created By  : Sajal Gupta on 23-11-2016
        /// Description : Send SMS to seller for notifying expiry of listing.
        /// </summary>
        public void CapitalFirstLoanSms(string number, string pageUrl, EnumSMSServiceType esms, string message)
        {
            try
            {
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, esms, pageUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.ExpiringListingReminderSMS({0},{1},{2},{3})", number, pageUrl, esms, message));
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 18th Jan 2018
        /// Description :Send SMS to customer on requesting dealer showroom details.
        /// Modiefier   : Kartik Rathod on 30 apl 2018,  Change formatting for dealer address, make static method
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="city"></param>
        /// <param name="pageUrl"></param>
        public static void DealerShowroomDetailsSMS(SMSData objSMSData, MobileSmsVerification objMobileVerification)
        {
            try
            {
                UrlShortner objUrlShortner = new UrlShortner();

                var shortUrlObj = objUrlShortner.GetShortUrl(string.Format("https://maps.google.com/?saddr=&daddr={0},{1}", objSMSData.Latitude, objSMSData.Longitude));
                string shortUrl = shortUrlObj != null ? shortUrlObj.ShortUrl : "";
                StringBuilder messageBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(objSMSData.Name))
                {
                    messageBuilder.Append(objSMSData.Name);
                    messageBuilder.Append(",");
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!String.IsNullOrEmpty(objSMSData.Address))
                {
                    messageBuilder.Append(objSMSData.Address);
                    messageBuilder.Append(",");
                }
                if (!String.IsNullOrEmpty(objSMSData.Area))
                {
                    messageBuilder.Append(objSMSData.Area);
                    messageBuilder.Append(",");
                }
                if (!String.IsNullOrEmpty(objSMSData.CityName))
                {
                    messageBuilder.Append(objSMSData.CityName);
                    messageBuilder.Append("-");
                }
                if (!String.IsNullOrEmpty(objSMSData.PinCode))
                {
                    messageBuilder.Append(objSMSData.PinCode);
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!String.IsNullOrEmpty(objSMSData.Phone))
                {
                    messageBuilder.Append("+91-");
                    messageBuilder.Append(objSMSData.Phone);
                    messageBuilder.Append(Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(shortUrl))
                {
                    messageBuilder.Append("Get Directions-");
                    messageBuilder.Append(shortUrl);
                    messageBuilder.Append(Environment.NewLine);
                }
                messageBuilder.Append("Thanks for visiting BikeWale.");
                string message = messageBuilder.ToString();

                EnumSMSServiceType esms = EnumSMSServiceType.DealerShowroomDetailsSMSToCustomer;
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(objMobileVerification.MobileNumber, message, esms, objMobileVerification.PageUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.DealerShowroomDetailsSMS({0},{1},{2},{3})", objMobileVerification.MobileNumber,
                    objMobileVerification.PageUrl, objSMSData.CityName, objSMSData.Phone));
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 May 2018
        /// Description :   Capital First SMS
        /// </summary>
        /// <param name="number"></param>
        /// <param name="pageUrl"></param>
        /// <param name="smsType"></param>
        /// <param name="message"></param>
        public void CapitalFirstSMS(string number, string pageUrl, EnumSMSServiceType smsType, string message)
        {
            try
            {
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, smsType, pageUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Notifications.CapitalFirstSMS({0},{1},{2},{3})", number, pageUrl, smsType, message));
            }
        }

    }   //End of class
}   //End of namespace
