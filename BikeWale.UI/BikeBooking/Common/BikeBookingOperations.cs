using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCClientInq.Proxy;

namespace Bikewale.BikeBooking.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 6 May 2015
    /// </summary>
    public class BikeBookingOperations
    {
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
                string jsonInquiryDetails = "{\"CustomerName\":\"" + customerName + "\", \"CustomerMobile\":\"" + customerMobile + "\", \"CustomerEmail\":\"" + customerEmail + "\", \"VersionId\":\"" + versionId + "\", \"CityId\":\"" + cityId + "\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}";

                TCApi_Inquiry objInquiry = new TCApi_Inquiry();
                abInquiryId = objInquiry.AddNewCarInquiry(branchId, jsonInquiryDetails);

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
        }   // End of PushInquiryInAB


    }    // class
}   // namespace