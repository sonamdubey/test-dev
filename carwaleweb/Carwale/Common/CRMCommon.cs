using Carwale.BL.PriceQuote;
using Carwale.Notifications;
using Carwale.Webservices.CRM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.UI.Common
{
    public class CRMCommon
    {
        string leadId = "-1";
        string _sourceId = "1";

        public CRMCommon()
        {
            _sourceId = ConfigurationManager.AppSettings["LeadSourceId"];
        }       
        /// <summary>
        /// Overloaded function, by Rajeev ON 17/12/2012 to resolve LA bug.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="phone"></param>
        /// <param name="customerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="carName"></param>
        /// <param name="price"></param>
        /// <param name="insurance"></param>
        /// <param name="rto"></param>
        /// <param name="inquiryId"></param>
        /// <param name="buyTime"></param>
        /// <param name="leadSourceCategoryId"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="leadSourceName"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public string SendCarData(string makeId, string fName, string lName, string email, string mobile, string phone,
                                    string customerId, string cityId, string versionId, string carName, string price,
                                            string insurance, string rto, string inquiryId, string buyTime,
                                            string leadSourceCategoryId, string leadSourceId, string leadSourceName, string modelId)
        {
            return SendCarData(makeId, fName, lName, email, mobile, phone, customerId, cityId, versionId, carName, price,
                                                insurance, rto, inquiryId, buyTime, leadSourceCategoryId, leadSourceId, leadSourceName, modelId, "");

        }

        /// <summary>
        /// Send Car Data in xmlstring to CRM Webservice
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="phone"></param>
        /// <param name="customerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="carName"></param>
        /// <param name="price"></param>
        /// <param name="insurance"></param>
        /// <param name="rto"></param>
        /// <param name="inquiryId"></param>
        /// <param name="buyTime"></param>
        /// <param name="leadSourceCategoryId"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="leadSourceName"></param>
        /// <param name="modelId"></param>
        /// <param name="visitedDealership">Price quote request page: 1 if user visited dealership, 0 if not and blank if user haven't selected anything</param>
        /// <returns></returns>
        public string SendCarData(string makeId, string fName, string lName, string email, string mobile, string phone,
                                    string customerId, string cityId, string versionId, string carName, string price,
                                            string insurance, string rto, string inquiryId, string buyTime,
                                            string leadSourceCategoryId, string leadSourceId, string leadSourceName, string modelId, string visitedDealership)
        {
            HttpContext.Current.Trace.Warn("visitedDealership2 : " + visitedDealership);
            try
            {
                lName = lName == "" ? "-" : lName;
                buyTime = buyTime == "" ? "1" : buyTime;

                //push data to CarWale CRM
                string xmlStr = "";
                xmlStr += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + "<CRMData>"
                            + "<VerificationData>"
                                + "<CityId>" + cityId + "</CityId>"
                                + "<MakeId>" + makeId + "</MakeId>"
                                + "<VersionId>" + versionId + "</VersionId>"
                                + "<ModelId>" + modelId + "</ModelId>"
                            + "</VerificationData>"
                            + "<CustomerData>"
                                + "<FirstName>" + RemoveSpecialCharacters(fName) + "</FirstName>"
                                + "<LastName>" + RemoveSpecialCharacters(lName) + "</LastName>"
                                + "<Email>" + RemoveSpecialCharacters(email) + "</Email>"
                                + "<Landline>" + RemoveSpecialCharacters(phone) + "</Landline>"
                                + "<Mobile>" + RemoveSpecialCharacters(mobile) + "</Mobile>"
                                + "<CWCustId>" + customerId + "</CWCustId>"
                                + "<CityId>" + cityId + "</CityId>"
                                + "<Source>" + _sourceId + "</Source>"
                                + "<LeadSourceCategoryId>" + leadSourceCategoryId + "</LeadSourceCategoryId>"
                                + "<LeadSourceId>" + leadSourceId + "</LeadSourceId>"
                                + "<LeadSourceName>" + leadSourceName + "</LeadSourceName>"
                            + "</CustomerData>"
                            + "<CarData>"
                                + "<CarVersionId>" + versionId + "</CarVersionId>"
                                + "<CarName>" + RemoveSpecialCharacters(carName) + "</CarName>"
                                + "<ExShowroomPrice>" + price + "</ExShowroomPrice>"
                                + "<Insurance>" + insurance + "</Insurance>"
                                + "<RTO>" + rto + "</RTO>"
                                + "<CityId>" + cityId + "</CityId>"
                                + "<ExpectedBuyingDate>" + DateTime.Now.AddDays(Convert.ToInt32(buyTime)) + "</ExpectedBuyingDate>"
                                + "<PQId>" + inquiryId + "</PQId>"
                                + "<IsVisitedDealership>" + visitedDealership + "</IsVisitedDealership>"
                            + "</CarData>"
                        + "</CRMData>";
                HttpContext.Current.Trace.Warn("xmlStr:" + xmlStr);
                PushCRM crm = new PushCRM();
                leadId = crm.PushToCRM(xmlStr, "1");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Common.SendCarData  : " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "Common.SendCarData");
                objErr.SendMail();
            }
            HttpContext.Current.Trace.Warn("leadId:" + leadId);
            return leadId;
        }
        
        string RemoveSpecialCharacters(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;");
        }        

    }//class
}