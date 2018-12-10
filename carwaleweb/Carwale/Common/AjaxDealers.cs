using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using AjaxPro;
using Carwale.BL.TCApi_Inquiry;
using System.Collections;
using Carwale.UI.NewCars;

namespace CarwaleAjax
{
	public class AjaxDealers
	{

        /// <summary>
        /// Model Offer Details Request, Customer data was sent to saved and Mail notification has sent to Dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="username"></param>
        /// <param name="mobile"></param>
        /// <param name="emailId"></param>
        /// <param name="comments"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmail"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool SendOfferDetails(string dealerId, string username, string mobile, string emailId, string comments, string dealerName, string dealerEmail)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(emailId) && !string.IsNullOrEmpty(comments))
            {
                string NCdealerId = dealerId;
                string userName = username.Trim();
                string number = mobile.Trim();
                string email = emailId.Trim();
                string modelName = comments.Trim();
                string dealer = dealerName.Trim();
                string dealerEmailId = dealerEmail.Trim();


                Hashtable htoffer = new Hashtable();
                htoffer.Add("dealerId", NCdealerId);
                htoffer.Add("username", userName);
                htoffer.Add("mobile", number);
                htoffer.Add("emailId", email);
                htoffer.Add("sourceId", "1");
                htoffer.Add("comments", modelName);

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonStr = serializer.Serialize((object)htoffer);
                //TCApi_Inquiry opr = new TCApi_Inquiry();
                TCApi_InquirySoapClient opr = new TCApi_InquirySoapClient();
                string res = opr.SaveOtherRequests(jsonStr);
                if (res == "1")
                {                 
                    Mails.NewcarDealerOfferInquiry(dealerEmailId, dealer, userName, number, emailId, modelName);
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        /* PRASAD: COMMENTED IT AS IT IS NOT CALLED FROM ANYWHERE IN THE CODE BASE
        /// <summary>
        /// Test Drive Request for Dealer has saved and Lead generated
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="custMob"></param>
        /// <param name="modelId"></param>
        /// <param name="requestDate"></param>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="dealerName"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool NewCarRequest(string customerName, string custMob, string modelId, string requestDate, string cityId, string dealerId, string dealerEmailId, string dealerName, string modelName, string requestType, string email, string registrationNo,string versionId,string inquirySourceId,string comments)
        {
            int selectedDealerId = -1;
            Int32.TryParse(dealerId, out selectedDealerId);
            if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(custMob) && !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(cityId) && !string.IsNullOrEmpty(dealerId) && selectedDealerId > 0)
            {
                string userName = customerName.Trim();
                string number = custMob.Trim();
                string preferredDate =  !string.IsNullOrEmpty(requestDate) ? Convert.ToDateTime(requestDate).ToString("MM/dd/yyyy hh:mm") + ":00.000" : DateTime.Now.ToString();
                //string preferredDate = requestDate;
                string dealer = dealerName.Trim();
                string dealerEmail = dealerEmailId.Trim();
                string commentsFromUser = (!String.IsNullOrEmpty(comments)) ? comments : preferredDate;
                Operations op = new Operations(dealerId);
                DealerDetails dealerData = new DealerDetails(dealerId);
                
                //string newversionId = string.Empty;
                if (versionId == "0" || versionId == "-1" || string.IsNullOrEmpty(versionId))
                {
                    versionId = op.GetTopVersion(modelId);
                }
                string Leadid = op.AddNewInquiries(userName, email, number, cityId, string.Empty, preferredDate, modelId, versionId, string.Empty, string.Empty, requestType, commentsFromUser, false, string.Empty, registrationNo, inquirySourceId);
                if (Leadid != "0")
                {
                    if (!string.IsNullOrEmpty(dealerEmail))
                    {
                        Mails.NewcarDealershowrroomInquiry(dealerEmail, dealer, userName, number, modelName);
                        
                        //send SMS
                        string message = "";
                        string url = "carwale.com";
                        string dealerMobile = dealerData.Mobile;
                        SMSCommon sms = new SMSCommon();
                        message = userName + " (" + number + ") has inquired to Know more about "+ modelName +" offers regarding '" + "Price Quote" + "' on " + url;

                        EnumSMSServiceType esms = EnumSMSServiceType.NewCarQuote;
                        sms.ProcessSMS(dealerMobile, message, esms, url);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        */

        /// <summary>
        /// User's can contact dealer by submitting the customer name & mobile along with date and time
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="username"></param>
        /// <param name="mobile"></param>
        /// <param name="comments"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmail"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool SendUserRequest(string dealerId, string username, string mobile, string comments, string dealerName, string dealerEmail)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(comments))
            {
                string UCdealerId = dealerId;
                string userName = username.Trim();
                string mobNumber = mobile.Trim();
                string Preferreddate = comments.Trim();
                string dealer = dealerName.Trim();
                string dealerEmailId = dealerEmail.Trim();


                Hashtable htoffer = new Hashtable();
                htoffer.Add("dealerId", UCdealerId);
                htoffer.Add("username", userName);
                htoffer.Add("mobile", mobNumber);
                htoffer.Add("sourceId", "1");
                htoffer.Add("comments", Preferreddate);

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonStr = serializer.Serialize((object)htoffer);
                //TCApi_Inquiry opr = new TCApi_Inquiry();
                TCApi_InquirySoapClient opr = new TCApi_InquirySoapClient();
                string res = opr.SaveOtherRequests(jsonStr);
                if (res == "1")
                {
                    Mails.UsedCarDealerInquiry(dealerEmailId, dealer, userName, mobNumber, Preferreddate);
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

	}
}