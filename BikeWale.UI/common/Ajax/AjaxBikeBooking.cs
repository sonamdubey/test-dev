using Bikewale.BAL.MobileVerification;
using Bikewale.BikeBooking.Common;
using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.MobileVerification;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using TCClientInq.Proxy;

namespace Bikewale.Ajax
{
    public class AjaxBikeBooking
    {
        static readonly IUnityContainer _container;
        static AjaxBikeBooking()
        {
            _container = new UnityContainer();
            _container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
            _container.RegisterType<IMobileVerificationRepository, MobileVerification>();
            _container.RegisterType<IMobileVerification, MobileVerification>();
        }

        // Marked unused By : Sadhana Upadhyay
        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Oct 2014
        /// Summmary : to update isverified flag in pq_newbikedealerpriceQuote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool UpdateIsMobileVerified(UInt32 pqId, string customerMobile, string customerEmail, string cwiCode, string branchId, string customerName, string versionId, string cityId)
        {
            bool isSuccess = false;
            try
            {
                IDealerPriceQuote objDealer = _container.Resolve<IDealerPriceQuote>();                
                IMobileVerificationRepository mobileVerRespo = _container.Resolve<IMobileVerificationRepository>();

                if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                {
                    if (mobileVerRespo.VerifyMobileVerificationCode(customerMobile, cwiCode, string.Empty))
                    {
                        isSuccess = objDealer.UpdateIsMobileVerified(pqId);

                        // if mobile no is verified push lead to autobiz
                        if (isSuccess)
                        {
                            BikeBookingOperations.PushInquiryInAB(branchId, pqId, customerName, customerMobile, customerEmail, versionId, cityId);
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

        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Oct 2014
        /// Summary :  to update mobile no. table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="customerMobile"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool UpdateMobileNumber(UInt32 pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId, string branchId)
        {
            bool isSuccess = false;
            bool isVerified = false;
            MobileVerificationEntity mobileVer = null;
            try
            {
                IDealerPriceQuote objDealer = _container.Resolve<IDealerPriceQuote>();
                IMobileVerificationRepository mobileVerRespo = _container.Resolve<IMobileVerificationRepository>();
                IMobileVerification mobileVerificetion = _container.Resolve<IMobileVerification>();

                if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                {
                    mobileVer = mobileVerificetion.ProcessMobileVerification(customerEmail, customerMobile);

                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(mobileVer.CustomerMobile, customerName, mobileVer.CWICode, HttpContext.Current.Request.ServerVariables["URL"].ToString());
                }
                else
                {
                    isSuccess = objDealer.UpdateIsMobileVerified(pqId);
                    isVerified = true;

                    // If customer is mobile verified push lead to autobiz
                    if (isVerified)
                    {
                        BikeBookingOperations.PushInquiryInAB(branchId, pqId, customerName, customerMobile, customerEmail, versionId, cityId);
                    }
                }

                isSuccess = objDealer.UpdateMobileNumber(pqId, customerMobile);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return isVerified;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Oct 2014
        /// Summary : to resend verification code
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool ResendVerificationCode(string customerName, string customerMobile, string customerEmail)
        {
            bool isSuccess = false;
            MobileVerificationEntity mobileVer = null;
            try
            {

                IMobileVerificationRepository mobileVerRespo = _container.Resolve<IMobileVerificationRepository>();
                IMobileVerification mobileVerificetion = _container.Resolve<IMobileVerification>();

                if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                {
                    mobileVer = mobileVerificetion.ProcessMobileVerification(customerEmail, customerMobile);

                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(mobileVer.CustomerMobile, customerName, mobileVer.CWICode, HttpContext.Current.Request.ServerVariables["URL"].ToString());

                    isSuccess = true;
                }


            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 18 Nov 2014
        /// Summary : To push Valid pq in autobiz
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="jsonInquiryDetails"></param>
        /// <param name="pqId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool PushInquiryInAB(string branchId, string jsonInquiryDetails, uint pqId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;

            try
            {
                TCApi_Inquiry objInquiry = new TCApi_Inquiry();
                abInquiryId = objInquiry.AddNewCarInquiry(branchId, jsonInquiryDetails);

                if (!String.IsNullOrEmpty(abInquiryId))
                {
                    IDealerPriceQuote objDealer = _container.Resolve<IDealerPriceQuote>();
                    isSuccess = objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Nov 2014
        /// Summary : to check new bike pq exist or not
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool IsNewBikePQExists(uint pqId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;
            try
            {
                IDealerPriceQuote objDealer = _container.Resolve<IDealerPriceQuote>();
                isSuccess = objDealer.IsNewBikePQExists(pqId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 16 Dec 2014
        /// Summary : To update bike color in dealer price quote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="colorId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool UpdatePQBikeColor(uint pqId, uint colorId)
        {
            bool isUpdated = false;

            try
            {
                IDealerPriceQuote objDealer = _container.Resolve<IDealerPriceQuote>();
                isUpdated = objDealer.UpdatePQBikeColor(colorId, pqId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return isUpdated;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 Jan 2015
        /// Summary : To check whether dealer prices available or not
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool IsDealerPricesAvailable(uint versionId, uint cityId)
        {
            bool isDealerPricesAvailable = false;
            try
            {
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                isDealerPricesAvailable = objDealer.IsDealerPriceAvailable(versionId, cityId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return isDealerPricesAvailable;
        }


        /// <summary>
        /// Written BY : Ashish G. Kamble on 16 June 2015
        /// Summary : Function to process the dealer price quote. 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="modelId"></param>
        /// <param name="isMobileSource"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string ProcessPQ(uint cityId, uint areaId, uint modelId, bool isMobileSource)
        {
            string response = string.Empty;

            Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
            try
            {

                    IDealerPriceQuote objIPQ = _container.Resolve<IDealerPriceQuote>();

                    Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity();
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                    objPQEntity.ClientIP = CommonOpn.GetClientIP();
                    objPQEntity.SourceId = isMobileSource ? Convert.ToUInt16(Bikewale.Common.Configuration.MobileSourceId) : Convert.ToUInt16(Bikewale.Common.Configuration.SourceId);
                    objPQEntity.ModelId = modelId;

                    // If pqId exists then, set pqId
                    objPQOutput = objIPQ.ProcessPQV3(objPQEntity);
            }
            catch (Exception ex)
            {
                string selectedParams = "cityid : " + cityId + ", areaId : " + areaId + ", modelId : " + modelId;
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " " + selectedParams);
                
            }     

            response = "{\"quoteId\":\"" + objPQOutput.PQId + "\",\"dealerId\":\"" + objPQOutput.DealerId + "\"}";

            return response;
        }

    }   //End of Class
}   //End of namespace