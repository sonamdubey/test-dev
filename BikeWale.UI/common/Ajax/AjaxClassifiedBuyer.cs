using System;
using System.Web;
using System.Text;
using AjaxPro;
using Bikewale.Common;
using Bikewale.Used;
using Bikewale.CV;
using Bikewale.Mails;

namespace Bikewale.Ajax
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 28/8/2012
    ///     Class for the ajax related functions for sell bikes
    /// </summary>
    public class AjaxClassifiedBuyer
    {
        [AjaxPro.AjaxMethod()]
        public string ShownInterestInThisBike(string inquiryId, string isDealer)
        {
            bool _IsDealer = isDealer == "1" ? true : false;
            bool shownInterest = false;
            // Generate JSON string manually
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            try
            {
                ClassifiedBuyerDetails objCBD = new ClassifiedBuyerDetails();
                objCBD.GetBuyerDetails();

                if (objCBD.BuyerId != "")
                {
                    shownInterest = Classified.HasShownInterestInUsedBike(_IsDealer, inquiryId, objCBD.BuyerId);
                }

                sb.Append("\"ShownInterest\":\"" + shownInterest + "\",");

                if (shownInterest)
                {
                    ClassifiedSellerDetails objSeller = new ClassifiedSellerDetails();
                    objSeller.GetSellerDetails(inquiryId, _IsDealer);

                    sb.Append("\"SellerName\":\"" + objSeller.SellerName + "\",");
                    sb.Append("\"SellerEmail\":\"" + objSeller.SellerEmail + "\",");
                    sb.Append("\"SellerContact\":\"" + objSeller.SellerContact + "\",");
                    sb.Append("\"SellerContactPerson\":\"" + objSeller.SellerContactPerson + "\",");
                    sb.Append("\"SellerAddress\":\"" + objSeller.SellerAddress + "\"");
                }
                else
                {
                    sb.Append("\"BuyerName\":\"" + objCBD.BuyerName + "\",");
                    sb.Append("\"BuyerMobile\":\"" + objCBD.BuyerMobile + "\",");
                    sb.Append("\"BuyerEmail\":\"" + objCBD.BuyerEmail + "\"");
                }

                sb.Append("}");
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_ShownInterestInThisBike");
                objErr.SendMail();
            }

            return sb.ToString();
        }


        [AjaxPro.AjaxMethod()]
        public bool IsBuyerMobileVerified(string custName, string eMail, string mobile)
        {
            bool isVerified = false;

            try
            {
                // Validate buyers information in case its not valid return false.
                if (custName != "" && Validations.IsValidEmail(eMail) && Validations.IsValidMobile(mobile))
                {
                    CustomerVerification cv = new CustomerVerification();
                    isVerified = cv.IsMobileVerified(custName, eMail, mobile);
                }

            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_IsBuyerMobileVerified" +
                                                    custName + ":" + eMail + ":" + mobile);
                objErr.SendMail();
            }
            return isVerified;
        }

        //
        // Ajax function to save request shown by buyer to any Bike listed for sell @ BikeWale
        // This function also sends Mail/SMS to both parties(Seller/Buyer)
        //
        [AjaxPro.AjaxMethod()]
        public string ProcessUsedBikePurchaseInquiry(string profileId, string buyerName, string buyerEmail, string buyerMobile, string bikeModel, string makeYear, string pageUrl)
        {
            bool _isDetailsViewed = true;
            string retData = "";
            try
            {
                PurchaseInquiries purInq = new PurchaseInquiries();
                retData = purInq.ProcessUsedBikePurchaseInquiry(profileId, buyerName, buyerEmail, buyerMobile, _isDetailsViewed, bikeModel, makeYear, pageUrl);
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_ProcessUsedBikePurchaseInquiry" +
                                                    profileId + ":" + buyerName + ":" + buyerEmail + ":" + buyerMobile + ":" +
                                                        _isDetailsViewed + ":" + bikeModel + ":" + makeYear + ":" + pageUrl);
                objErr.SendMail();
            }

            return retData;
        }

        //
        // This ajax function used to veryfy 5-digit verification code send to buyers mobile number
        // This function will return trut if matched successfully alse will retuen false;
        // Function accecpts two parameters
        // 1. Buyers mobile number
        // 2. Code received by buyer on there mobile.
        //
        [AjaxPro.AjaxMethod()]
        public bool CheckVerificationCode(string mobile, string cwiCode)
        {
            bool isVerified = false;

            try
            {
                CustomerVerification cv = new CustomerVerification();
                isVerified = cv.CheckVerification(mobile, cwiCode, "");
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_CheckVerificationCode" +
                                                    mobile + ":" + cwiCode);
                objErr.SendMail();
            }

            return isVerified;
        }

         
         //This function returns seller information in JSON format
         //Gets following arguments
         //BikeId : id of the Bike listed for sell
         //isDealer : true if its a dealer
        
        [AjaxPro.AjaxMethod()]
        public bool UploadPhotosRequest(string sellInquiryId, string consumerType, string buyerMessage, string buyerName, string buyerEmail, string buyerMobile, string bikeName)
        {
            bool isDone = false;

            try
            {
                if (CommonOpn.CheckId(sellInquiryId))
                {
                    string buyerId = "";

                    // Check if buyer information already exists with cookies
                    ClassifiedBuyerDetails objCBD = new ClassifiedBuyerDetails();
                    objCBD.GetBuyerDetails();
                    buyerId = objCBD.BuyerId;

                    // If buyer not exists with cookies, Perform automated registration
                    if (buyerId == "")
                    {
                        AjaxClassifiedBuyer ajBuyer = new AjaxClassifiedBuyer();

                        RegisterCustomer objRC = new RegisterCustomer();
                        string CustomerId = objRC.RegisterUser(buyerName, buyerEmail, buyerMobile, "", "", "");

                        if (!String.IsNullOrEmpty(CustomerId))
                        {
                            buyerId = CustomerId;

                            string buyerData = buyerName + ":" + buyerMobile + ":" + buyerEmail + ":" + BikewaleSecurity.EncryptUserId(long.Parse(CustomerId));
                            objCBD.SetBuyerDetails(buyerData);
                        }
                    }

                    if (buyerId != "" && buyerId != "-1")
                    {
                        ClassifiedInquiryPhotos objPhotos = new ClassifiedInquiryPhotos();
                        isDone = objPhotos.UploadPhotosRequest(sellInquiryId, buyerId, consumerType, buyerMessage);

                        // If buyer's request to seller successfully saved to database
                        // Send mail to inform seller
                        if (isDone)
                        {
                            bool isDealer = consumerType == "1" ? true : false;

                            ClassifiedSellerDetails objSeller = new ClassifiedSellerDetails();
                            objSeller.GetSellerDetails(sellInquiryId, isDealer);

                            if (objSeller.SellerEmail != "" && objSeller.SellerName != "")
                            {
                                string subject = "Upload Bike Photos";
                                string profileId = (consumerType == "1" ? "D" : "S") + sellInquiryId;
                                string listingUrl = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/used/sell/uploadbasic.aspx?id=" + profileId;
                                string mailBody = "";

                                if (isDealer)
                                {
                                    mailBody = ClassifiedMailContent.PhotoRequestToDealerSeller(objSeller.SellerName, buyerName, buyerMobile, bikeName, profileId);
                                }
                                else
                                {
                                    mailBody = ClassifiedMailContent.PhotoRequestToIndividualSeller(objSeller.SellerName, buyerName, buyerMobile, bikeName, listingUrl);
                                }

                                //MailServices objMails = new MailServices();

                                // Send mail to myself also. just for testing purpose
                                //objMails.SendMail("satishdixit@gmail.com", subject + " : " + objSeller.SellerEmail, mailBody, true);
                                //objMails.SendMail(objSeller.SellerEmail, subject, mailBody, true);
                                CommonOpn op = new CommonOpn();
                                op.SendMail(objSeller.SellerEmail, subject, mailBody, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_UploadPhotosRequest");
                objErr.SendMail();
            }
            return isDone;
        }   // End of UploadPhotoRequest method


    }   // End of class
}   // end of namespace