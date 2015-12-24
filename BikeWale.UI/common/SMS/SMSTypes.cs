using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Bikewale.Common
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class SMSTypes
    {
        public void SMSToSeller(string profileId, string buyerName, string buyerMobile,
                                    bool showDetails, string bikeModel, string makeYear, string pageUrl)
        {
            //check whether the seller is an individual or a dealer
            bool isDealer = CommonOpn.CheckIsDealerFromProfileNo(profileId);
            string sellInqId = CommonOpn.GetProfileNo(profileId);

            //now get the seller mobile number from the profile id
            Database db = new Database();
            SqlDataReader dr = null;
            string sql = "", sellerMobile = "";
            try
            {
                if (isDealer == true)
                    sql = " Select D.MobileNo AS Mobile From Dealers AS D, SellInquiries AS SI With(NoLock) "
                        + " Where SI.ID = @sellInqId AND D.ID = SI.DealerId ";
                else
                    sql = " Select C.Mobile AS Mobile From Customers AS C, ClassifiedIndividualSellInquiries AS SI With(NoLock) "
                        + " Where SI.ID = @sellInqId AND C.ID = SI.CustomerId ";

                SqlParameter[] param = { new SqlParameter("@sellInqId", sellInqId) };
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    sellerMobile = dr["Mobile"].ToString();
                }

                db.CloseConnection();

                // old template commented by Ashish on 15/9/2012
                //string message = "New Enquiry on BikeWale for your " + makeYear + "-" + bikeModel + ": " + buyerName + " " + buyerMobile + ".";
                string message = "New inquiry on BikeWale for your " + bikeModel + ":" + buyerName + buyerMobile + ".";

                if (isDealer == false)
                    //message += " SMS 'REMOVE' to 56767767 to remove your advertisement from bikeWale.";
                    //message += " SMS SOLD to 56767767 to remove your ad from BikeWale.";
                    message += " Visit www.bikewale.com/mybikewale/ to manage your ad.";
                else
                    message += " Visit www.bikewale.com/dealers for more details.";


                EnumSMSServiceType esms = isDealer == true ?
                                                    EnumSMSServiceType.UsedPurchaseInquiryDealerSeller :
                                                    EnumSMSServiceType.UsedPurchaseInquiryIndividualSeller;


                HttpContext.Current.Trace.Warn("Sending SMS To Seller : " + message);

                SMSCommon sc = new SMSCommon();

                sc.ProcessSMS(sellerMobile, message, esms, pageUrl, true);

            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSCommon");
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
        }

        public void SMSToBuyer(string profileId, string buyerMobile, string sellerName,
                                    string sellerContact, bool showDetails, string bikeModel, string makeYear, string pageUrl)
        {
            //check whether the seller is an individual or a dealer
            bool isDealer = CommonOpn.CheckIsDealerFromProfileNo(profileId);

            try
            {
                string message = "";

                //if bike belongs to Mumbai & NCR
                if (CommonOpn.CheckForMumbai(profileId))
                {
                    if (showDetails == true)
                    {
                        //message = "For \"" + bikeModel + "\" you selected at BikeWale, call its seller "
                        //        + sellerName + " at " + sellerContact + ". Visit www.bikewale.com/MyBikeWale/ for more details.";

                        message = "For " + bikeModel + " you selected at BikeWale, call its seller "
                                + sellerName + " at " + sellerContact + ". Visit www.bikewale.com/MyBikeWale/ for more details.";

                        /*
                        + (isDealer == false ? " To get finance on this bike call Kshitij at 9821651116" : "");*/
                    }
                    else
                    {
                        //message = "Get seller details for \"" + makeYear + "-" + bikeModel + "\" for just Rs.500.\n"
                        //+ "Pay online at www.bikeWale.com/MybikeWale/ or call 022-67398888 for help";  //32651254, 32651255

                    }
                }
                else//bike belongs to other cities.
                {
                    //if(showDetails == true)
                    //{
                    //message = "For \"" + bikeModel + "\" you selected at BikeWale, call its seller "
                    //        + sellerName + " at " + sellerContact + ". Visit www.BikeWale.com/MyBikeWale/ for more details";
                    message = "For " + bikeModel + " you selected at BikeWale, call its seller "
                                + sellerName + " at " + sellerContact + ". Visit www.bikewale.com/MyBikeWale/ for more details.";
                    /*}
                    else
                    {
                        message = "Get seller details for \"" + makeYear + "-" + bikeModel + "\" for just Rs.500.\n"
                                + "Pay online at www.bikeWale.com/MybikeWale/ or call 022-67398888 for help";  //32651254, 32651255
						
                    }*/
                }


                EnumSMSServiceType esms = EnumSMSServiceType.UsedPurchaseInquiryIndividualBuyer;

                HttpContext.Current.Trace.Warn("Sending SMS To Buyer : " + message);
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(buyerMobile, message, esms, pageUrl, true);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSCommon");
                objErr.SendMail();
            }
        }

        public void NewSellBikeRequest(string sellerName, string sellerContact, string profileId, string email, string pageUrl)
        {
            if (InOfficeTime() == true)
            {
                string numberToBeSentOn = GetTCNumber("1");//"9833499441";	//9867355378

                if (numberToBeSentOn != "")
                {
                    string message = "New Seller : " + sellerName + "/" + sellerContact + ". Profile Id " + profileId + ", " + email;

                    //sms the seller details to this number
                    EnumSMSServiceType esms = EnumSMSServiceType.NewBikeSellOpr;
                    SMSCommon sc = new SMSCommon();
                    sc.ProcessSMS(numberToBeSentOn, message, esms, pageUrl);
                }
            }
        }


        public void NewBuyBikeRequest(string buyerName, string buyerContact, string profileId, string email, string pageUrl)
        {
            /*if(InOfficeTime() == true)
            {
				
                //send it between the office hours
                string numberToBeSentOn = "9867355378";
				
                string message = "New Buyer : " + buyerName + "/" + buyerContact + ". Profile Id " + profileId + ", " + email;
				
                //sms the seller details to this number
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikeBuyOpr;
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(numberToBeSentOn, message, esms, pageUrl);
				
            }*/
        }


        bool InOfficeTime()
        {
            DateTime officeStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                                                    DateTime.Today.Day, 19, 0, 0);
            DateTime officeEnd = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                                                    DateTime.Today.Day, 23, 0, 0);

            bool retVal = true;

            if (DateTime.Now >= officeStart && DateTime.Now <= officeEnd)
                retVal = true;
            else
                retVal = false;

            //also check whether the week day is sunday
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday || DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                retVal = false;

            return retVal;
        }

        public void RegistrationSMS(string name, string password, string number, string eMail, string pageUrl)
        {
            try
            {
                string message = "";

                message = "Dear " + name + ", Thank you for your registration at BikeWale. Your password is " + password + " and your loginid is the eMail id you provided.";


                EnumSMSServiceType esms = EnumSMSServiceType.CustomerRegistration;

                HttpContext.Current.Trace.Warn("Sending SMS To Customer : " + message);
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(number, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.RegistrationSMS : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.RegistrationSMS");
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
                HttpContext.Current.Trace.Warn("Common.SMSDealerAddress : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSDealerAddress");
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
                HttpContext.Current.Trace.Warn("Common.SMSNewBikeQuote : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSNewBikeQuote");
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
            HttpContext.Current.Trace.Warn("SMSMobileVerification method");
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
                HttpContext.Current.Trace.Warn("Common.SMSNewBikeQuote : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSNewBikeQuote");
                objErr.SendMail();
            }
        }

        // FUNCTION TO SEND ADDRESS OF THE DEALER TO THE REQUESTED CUSTOMER FROM DEALER SHOWROOM
        private string GetTCNumber(string serviceType)
        {
            Database db = new Database();
            SqlDataReader dr = null;
            string sql = "";
            string number = "";
            try
            {
                sql = " Select Number From TeleCaller_SMS With(NoLock) Where ServiceType = @serviceType";

                SqlParameter[] param = { new SqlParameter("@serviceType", serviceType) };
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    number = dr["Number"].ToString();
                }
                dr.Close();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.GetTCNumber : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.GetTCNumber");
                objErr.SendMail();
            }
            finally
            {

                db.CloseConnection();
            }
            return number;
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

                string message = "";

                message = "BikeWale purchase enquiry: Please call " + customerName + ", " + areaName + ", " + cityName + " at " + customerMobile + " for " + BikeName + " and schedule customer visit.";

                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(dealerMobileNo, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.NewBikePriceQuoteSMSToDealer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.NewBikePriceQuoteSMSToDealer");
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
        public void NewBikePriceQuoteSMSToCustomer(string customerMobile, string customerName, string BikeName, string dealerName, string dealerContactNo, string dealerAddress, string pageUrl, uint bookingAmount, uint insuranceAmount = 0, bool hasBumperDealerOffer = false)
        {
            try
            {
                EnumSMSServiceType esms = EnumSMSServiceType.NewBikePriceQuoteSMSToCustomer;

                string message = "";

                //message = "Dear " + customerName + ", Thank you for showing interest in " + BikeName + ". Dealer details: " + dealerName + ", " + dealerContactNo + ", " + dealerAddress;
                if (!hasBumperDealerOffer)
                {
                    if (insuranceAmount == 0)
                    {
                        //message = String.Format("Pay Rs. {1} to book your {0} at BikeWale. Pay the balance amount at {2}({3}). Avail a helmet worth Rs. 1000 and one year RSA absolutely FREE by claiming it from BikeWale.", BikeName, bookingAmount,dealerName, dealerContactNo);
                        message = String.Format("Pay Rs. {0} on BikeWale to book your bike, pay balance amount at {1} {2} ({3}), and claim Free Helmet %26 1-year RSA from BikeWale.", bookingAmount, dealerName, dealerAddress, dealerContactNo);
                    }
                    else
                    {
                        message = String.Format("Pay Rs. {3} to book your {0} online at BikeWale %26 get 100%25 discount on Insurance at {1}({2})", BikeName, dealerName, dealerContactNo, bookingAmount);
                    }
                }
                else
                {
                    message = String.Format("Pay Rs. {0} to book your {1} at BikeWale to get free insurance, free accessories worth Rs. 3,000 and discount on bike worth Rs. 1,000 at the dealership!",bookingAmount,BikeName);
                }
                SMSCommon sc = new SMSCommon();
                sc.ProcessSMS(customerMobile, message, esms, pageUrl);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.NewBikePriceQuoteSMSToCustomer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.NewBikePriceQuoteSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 19 Dec 2014
        /// Summary    : PopulateWhere to send sms to customer after bike is booked successfully.
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
                HttpContext.Current.Trace.Warn("Common.BikeBookingSMSToCustomer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.BikeBookingSMSToCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 19 Dec 2014
        /// Summary    : PopulateWhere to send sms to dealer after user booked bike successfully.
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
                HttpContext.Current.Trace.Warn("Common.NewBikePriceQuoteSMSToDealer : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.NewBikePriceQuoteSMSToDealer");
                objErr.SendMail();
            }
        }
    }   // End of class
}   // End of namespace