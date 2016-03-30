using Bikewale.Notifications;
using System;

namespace PriceQuoteLeadSMS
{
    /// <summary>
    /// Modified by :   Sumit Kate on 29 Mar 2016
    /// Summary     :   AutoBiz Web Service Adaptor
    /// </summary>
    public class AutoBizAdaptor
    {
        /// <summary>
        ///  Written By : Ashish G. Kamble
        /// Summary : Function to push the inquiry to the autobiz. Lead should be pushed only if mobile number is verified.
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Summary     :   Send campaign Id
        /// </summary>
        /// <param name="branchId">Dealer id</param>
        /// <param name="pqId">Price quote Id</param>
        /// <param name="customerName">Name of the customer</param>
        /// <param name="customerMobile">mobile no of the customer</param>
        /// <param name="customerEmail">email id of the customer</param>
        /// <param name="versionId">bike version id</param>
        /// <param name="campaignId">dealer campaign id</param>
        /// <returns></returns>
        public static bool PushInquiryInAB(string branchId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId, string campaignId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;

            try
            {                
                string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", customerName, customerMobile, customerEmail, versionId, cityId, campaignId);

                TCApi_Inquiry objInquiry = new TCApi_Inquiry();
                abInquiryId = objInquiry.AddNewCarInquiry(branchId, jsonInquiryDetails);

                if (!String.IsNullOrEmpty(abInquiryId))
                {
                    if (abInquiryId != "0" && abInquiryId != "-1")
                    {
                        LeadSMSDAL leadSMSDal = new LeadSMSDAL();
                        isSuccess = leadSMSDal.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AutoBizAdaptor.PushInquiryInAB");
                objErr.SendMail();
            }
            return isSuccess;
        }
    }
}
