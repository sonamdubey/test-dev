using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.TCClientInq.Proxy;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;

namespace Bikewale.Service.TCAPI
{
    public class AutoBizAdaptor
    {
        private static string _abEnquiryApiUrl = string.Format("{0}{1}", ConfigurationManager.AppSettings["ABHostUrl"], ConfigurationManager.AppSettings["ABEnquiryApiUrl"]);
        /// <summary>
        ///  Written By : Ashish G. Kamble
        /// Summary : Function to push the inquiry to the autobiz. Lead should be pushed only if mobile number is verified.
        /// </summary>
        /// <param name="branchId">Dealer id</param>
        /// <param name="pqId">Price quote Id</param>
        /// <param name="customerName">Name of the customer</param>
        /// <param name="customerMobile">mobile no of the customer</param>
        /// <param name="customerEmail">email id of the customer</param>
        /// <param name="versionId">bike version id</param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static bool PushInquiryInAB(string branchId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;

            try
            {

                string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\",\"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\",\"BranchId\":\"{5}\"}}", customerName, customerMobile, customerEmail, versionId, cityId, branchId);

                using (BWHttpClient objClient = new BWHttpClient())
                {
                    abInquiryId = objClient.PostJsonSync<string>(APIHost.AB, BWConfiguration.Instance.APIRequestTypeJSON, _abEnquiryApiUrl, jsonInquiryDetails);
                }

                if (!String.IsNullOrEmpty(abInquiryId))
                {
                    if (abInquiryId != "0" && abInquiryId != "-1")
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                            IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                            isSuccess = objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }
            return isSuccess;
        }


        public static string PushInquiryInAB(string branchId, uint pqId, string customerName, string customerMobile, string customerEmail, uint versionId, string cityId)
        {
            string abInquiryId = string.Empty;

            try
            {
                string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\",\"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\",\"BranchId\":\"{5}\"}}", customerName, customerMobile, customerEmail, versionId, cityId, branchId);
                
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    abInquiryId = objClient.PostJsonSync<string>(APIHost.AB, BWConfiguration.Instance.APIRequestTypeJSON, _abEnquiryApiUrl, jsonInquiryDetails);
                }

                if (!String.IsNullOrEmpty(abInquiryId))
                {
                    if (abInquiryId != "0" && abInquiryId != "-1")
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                            IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                            objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }
            return abInquiryId;
        }
    }
}