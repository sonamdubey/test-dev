﻿using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.TCClientInq.Proxy;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.TCAPI
{
    public class AutoBizAdaptor
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }


        public static string PushInquiryInAB(string branchId, uint pqId, string customerName, string customerMobile, string customerEmail, uint versionId, string cityId)
        {
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

                            objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return abInquiryId;
        }
    }
}