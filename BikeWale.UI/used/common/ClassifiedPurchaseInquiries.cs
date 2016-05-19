using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Common;
using Bikewale.Ajax;

/// <summary>
///     Created By : Ashish G. Kamble
///     Class to process ClassifiedPurchaseInquiries
/// </summary>
/// 
namespace Bikewale.Used
{
    public class PurchaseInquiries
    {
	    public string ProcessUsedBikePurchaseInquiry( string profileId, string buyerName, string buyerEmail, string buyerMobile, bool showDetails, string bikeModel, string makeYear, string pageUrl )
		{
            string status = string.Empty, message = string.Empty, CustomerId = string.Empty;

            // Generate JSON string manually
			StringBuilder sb = new StringBuilder();
			sb.Append("{");

            try
            {
                // Validate buyer information
                if (buyerName != "" && Validations.IsValidEmail(buyerEmail.ToLower()) && Validations.IsValidMobile(buyerMobile))
                {
                    AjaxClassifiedBuyer ajBuyer = new AjaxClassifiedBuyer();

                    RegisterCustomer objRC = new RegisterCustomer();

                    CustomerId = objRC.IsRegisterdCustomer(buyerEmail.ToLower());

                    if (string.IsNullOrEmpty(CustomerId))
                        CustomerId = objRC.RegisterUser(buyerName, buyerEmail.ToLower(), buyerMobile, "", "", "");

                    if (ajBuyer.IsBuyerMobileVerified(buyerName, buyerEmail.ToLower(), buyerMobile))
                    {
                        // Update customer mobile number into customers table.
                        objRC.UpdateCustomerMobile(buyerMobile, buyerEmail.ToLower(), buyerName);

                        ClassifiedBuyerDetails cbd = new ClassifiedBuyerDetails();
                        if (cbd.IsBuyerEligible(buyerMobile))
                        {
                            cbd.GetBuyerDetails();
                            // Set buyer information in cookies
                            if (cbd.BuyerId == "")
                            {
                                string buyerData = buyerName + ":" + buyerMobile + ":" + buyerEmail.ToLower() + ":" + BikewaleSecurity.EncryptUserId(long.Parse(CustomerId));
                                cbd.SetBuyerDetails(buyerData);
                            }

                            bool isDealer = CommonOpn.CheckIsDealerFromProfileNo(profileId);

                            //based on the seller, submit the inquiry in the respective table
                            //first check whether this inquiry is for the dealer or for the individual
                            string purInquiryId = string.Empty;

                            if (isDealer)// if dealer
                            {
                                //purInquiryId = SubmitInquiryDealer(CustomerId, CommonOpn.GetProfileNo(profileId));

                                //// Send alerts to both parties
                                //SendAlertsDealerSeller(profileId, CustomerId, buyerName, buyerMobile, buyerEmail, pageUrl);
                            }
                            else
                            {
                                purInquiryId = SubmitInquiryCustomer(CustomerId, CommonOpn.GetProfileNo(profileId));

                                // Send alerts to both parties
                                SendAlertsIndividualSeller(profileId, CustomerId, buyerName, buyerMobile, buyerEmail.ToLower(), pageUrl, showDetails);                                
                            }

                            if (purInquiryId != "-1")
                            {
                                status = "1";
                                message = "Process completed successfully.";

                                ClassifiedSellerDetails objSeller = new ClassifiedSellerDetails();
                                objSeller.GetSellerDetails(CommonOpn.GetProfileNo(profileId), isDealer);

                                sb.Append("\"SellerName\":\"" + objSeller.SellerName + "\",");
                                sb.Append("\"SellerEmail\":\"" + objSeller.SellerEmail + "\",");
                                sb.Append("\"SellerContact\":\"" + objSeller.SellerContact + "\",");
                                sb.Append("\"SellerAddress\":\"" + objSeller.SellerAddress + "\",");
                                sb.Append("\"SellerContactPerson\":\"" + objSeller.SellerContactPerson + "\",");
                                
                            }
                        }
                        else
                        {
                            status = "5";
                            message = "Oops! You have reached the maximum limit for viewing inquiry details in a day.";
                        }
                    }
                    else
                    {
                        status = "3";
                        message = "Buyer mobile is not verified";
                    }
                }
                else // Buyer information is not valid, return a message
                {
                    status = "2";
                    message = "Information you provided was invalid. Please provide valid information.";
                }

                sb.Append("\"Status\":\"" + status + "\",");
                sb.Append("\"Message\":\"" + message + "\"");
                sb.Append("}"); // end of json formation
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : ProcessUsedBikePurchaseInquiry");
                objErr.SendMail();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Function to send EMAIL and SMS alerts to both buyers and sellers
        /// </summary>
        /// <param name="profileId">If of the car listed for sell and buyer shown interest</param>
        /// <param name="buyerId">Identity of buyer</param>
        /// <param name="buyerName">Buyer name</param>
        /// <param name="buyerMobile">Buyer mobile</param>
        /// <param name="buyerEmail">Buyer email</param>
        /// <param name="pageUrl">Url of the page from buyer initiated this request</param>
        //void SendAlertsDealerSeller(string profileId, string buyerId, string buyerName, string buyerMobile, string buyerEmail, string pageUrl)
        //{
        //    try
        //    {
        //        SMSTypes st = new SMSTypes();

        //        DealerSellInquiryDetails dsd = new DealerSellInquiryDetails(CommonOpn.GetProfileNo(profileId));

        //        //Send SMS to both buyer and seller
        //        st.SMSToSeller(profileId, buyerName, buyerMobile, true, dsd.CarName, dsd.MakeYear, pageUrl);
        //        st.SMSToBuyer(profileId, buyerMobile, dsd.SellerName, dsd.SellerContact, true, dsd.CarName, dsd.MakeYear, pageUrl);

        //        // Emamil seller details to buyer
        //        Bikewale.Common.Mails.SendSellerDetailsToBuyer(dsd.SellerEmail, dsd.SellerName, dsd.SellerContact, dsd.City, profileId, buyerId, dsd.CarName, dsd.Kilometers, dsd.MakeYear, CommonOpn.FormatNumeric(dsd.Price));

        //        //email of the dealer
        //        Bikewale.Common.Mails.ForwardUsedCarPurchaseInquiry(dsd.SellerEmail, dsd.SellerName, profileId, ""/*no message in this case*/, buyerId, ""/*comments*/, dsd.CarName, dsd.Kilometers, dsd.MakeYear, CommonOpn.FormatNumeric(dsd.Price));
        //    }
        //    catch (Exception ex)
        //    {
        //        //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
        //        HttpContext.Current.Trace.Warn(ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : SendAlertsDealerSeller");
        //        objErr.SendMail();
        //    }
        //}

        
        /// <summary>
        /// Indiviual Seller: Function to send EMAIL and SMS alerts to both buyers and sellers 
        /// </summary>
        /// <param name="profileId">If of the Bike listed for sell and buyer shown interest</param>
        /// <param name="buyerId">Identity of buyer</param>
        /// <param name="buyerName">Buyer name</param>
        /// <param name="buyerMobile">Buyer mobile</param>
        /// <param name="buyerEmail">Buyer email</param>
        /// <param name="pageUrl">Url of the page from buyer initiated this request</param>
        /// <param name="showDetails">bool: </param>
        string SendAlertsIndividualSeller(string profileId, string buyerId, string buyerName, string buyerMobile, string buyerEmail, string pageUrl, bool showDetails)
        {
            string msg = string.Empty;
            try
            {
                msg = "SendAlertsIndividualSeller method called";
                SMSTypes st = new SMSTypes();

                SellInquiryDetails csd = new SellInquiryDetails(CommonOpn.GetProfileNo(profileId));

                msg = csd.GetInquiryDetails(CommonOpn.GetProfileNo(profileId));

                //to the seller 
                st.SMSToSeller(profileId, buyerName, buyerMobile, showDetails, csd.BikeName, csd.MakeYear, pageUrl);

                //to the buyer
                st.SMSToBuyer(profileId, buyerMobile, csd.SellerName, csd.SellerMobile, showDetails, csd.BikeName, csd.MakeYear, pageUrl);

              
                Bikewale.Common.Mails.SendSellerDetailsToBuyer(csd.SellerEmail, csd.SellerName, csd.SellerMobile, csd.City, profileId, buyerId, csd.BikeName, csd.Kilometers, csd.MakeYear, CommonOpn.FormatNumeric(csd.Price));

                //send mail to the seller of the buyer info
                Bikewale.Common.Mails.ContactSeller(csd.SellerEmail, csd.SellerName, buyerId, "", profileId, csd.BikeName, csd.Kilometers, csd.MakeYear, CommonOpn.FormatNumeric(csd.Price));
              
            }
            catch (Exception ex)
            {
                msg = "SendAlertsIndividualSeller ex.Message : " + ex.Message;
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : SendAlertsIndividualSeller");
                objErr.SendMail();
            }
            return msg;
        }   // End of SendAlertsIndividualSeller method
        
        /// <summary>
        ///  Submit purchase inquiries for Individual
        /// Modified By : Sadhana Upadhyay on 2nd April 2014
        /// Summary : To capture Client IP
        /// </summary>
        /// <param name="customerId">used id of the buyer</param>
        /// <param name="sellInqId">id of the Bike used was intersted in(Primary key of CustomerSellInquiries table)</param>
        /// <returns>id of the latest inserted record in ClassifiedRequests table</returns>
        private static string SubmitInquiryCustomer(string customerId, string sellInqId)
        {
            throw new Exception("Method not used/commented");

            //string inqId = "-1";

            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();

            //string conStr = db.GetConString();
            //con = new SqlConnection(conStr);

            //try
            //{
            //    HttpContext.Current.Trace.Warn("Submitting Data");
            //    cmd = new SqlCommand("InsertClassifiedRequests", con);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    prm = cmd.Parameters.Add("@SellInquiryId", SqlDbType.BigInt);
            //    prm.Value = sellInqId;

            //    prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
            //    prm.Value = customerId;

            //    prm = cmd.Parameters.Add("@RequestDateTime", SqlDbType.DateTime);
            //    prm.Value = DateTime.Now;

            //    prm = cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 500);
            //    prm.Value = "";

            //    prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
            //    prm.Direction = ParameterDirection.Output;

            //    prm = cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40);
            //    prm.Value = CommonOpn.GetClientIP();

            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();

            //    inqId = cmd.Parameters["@InquiryId"].Value.ToString();

            //}
            //catch (SqlException err)
            //{
            //    //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();


            //} // catch SqlException
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
            //return inqId;
        }   // End of SubmitInquiryCustomer

             
    }   // End of class
}   // End of namespace