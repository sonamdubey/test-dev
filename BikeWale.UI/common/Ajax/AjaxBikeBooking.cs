using Bikewale.Interfaces.BikeBooking;
using Bikewale.BAL.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.BAL.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.BAL.MobileVerification;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Common;
using Bikewale.BikeBooking;
using TCClientInq.Proxy;
using Bikewale.Mobile.PriceQuote;
using Bikewale.BikeBooking.Common;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Entities.PriceQuote;
using System.Configuration;
using System.Data;
using Bikewale.Entities.BikeBooking;

namespace Bikewale.Ajax
{
    public class AjaxBikeBooking
    {
        // Marked unused By : Sadhana Upadhyay
#if unused
        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Oct 2014
        /// Summary : function to save customer detail
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool SaveCustomerDetail(UInt32 dealerId, UInt32 pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId)
        {
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            CustomerEntity objCust = null;
            MobileVerificationEntity mobileVer = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICustomerAuthentication<CustomerEntity, UInt32>, CustomerAuthentication<CustomerEntity, UInt32>>();
                    ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer = container.Resolve<ICustomerAuthentication<CustomerEntity, UInt32>>();

                    if (!objAuthCustomer.IsRegisteredUser(customerEmail))
                    {
                        container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                        ICustomer<CustomerEntity, UInt32> objCustomer = container.Resolve<ICustomer<CustomerEntity, UInt32>>();

                        Bikewale.Common.RegisterCustomer rc = new Bikewale.Common.RegisterCustomer();
                        password = rc.GenerateRandomPassword();
                        salt = rc.GenerateRandomSalt();
                        hash = rc.GenerateHashCode(password, salt);

                        objCust = new CustomerEntity() { CustomerName = customerName, CustomerEmail = customerEmail, CustomerMobile = customerMobile, PasswordSalt = salt, PasswordHash = hash, ClientIP = CommonOpn.GetClientIP() };
                        UInt32 CustomerId = objCustomer.Add(objCust);
                    }

                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isSuccess = objDealer.SaveCustomerDetail(dealerId, pqId, customerName, customerMobile, customerEmail,null);

                    DealerPriceQuoteCookie.CreateDealerPriceQuoteCookie(PriceQuoteCookie.PQId, false, false);
                    CustomerDetailCookie.CreateCustomerDetailCookie(customerName, customerEmail, customerMobile);

                    container.RegisterType<IMobileVerificationRepository, MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                    {
                        container.RegisterType<IMobileVerification, MobileVerification>();
                        IMobileVerification mobileVerificetion = container.Resolve<IMobileVerification>();

                        mobileVer = mobileVerificetion.ProcessMobileVerification(customerEmail, customerMobile);
                        isVerified = false;

                        SMSTypes st = new SMSTypes();
                        st.SMSMobileVerification(mobileVer.CustomerMobile, customerName, mobileVer.CWICode, HttpContext.Current.Request.ServerVariables["URL"].ToString());
                    }
                    else
                    {
                        isVerified = objDealer.UpdateIsMobileVerified(pqId);

                        // If customer is mobile verified push lead to autobiz
                        if(isVerified)
                        {
                            BikeBookingOperations.PushInquiryInAB(dealerId.ToString(), pqId, customerName, customerMobile, customerEmail, versionId, cityId);
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
            return isVerified;
        }
#endif
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    container.RegisterType<IMobileVerificationRepository, MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                    {
                        if (mobileVerRespo.VerifyMobileVerificationCode(customerMobile, cwiCode, ""))
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
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    container.RegisterType<IMobileVerificationRepository, MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    container.RegisterType<IMobileVerification, MobileVerification>();
                    IMobileVerification mobileVerificetion = container.Resolve<IMobileVerification>();

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
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IMobileVerificationRepository, MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    container.RegisterType<IMobileVerification, MobileVerification>();
                    IMobileVerification mobileVerificetion = container.Resolve<IMobileVerification>();

                    if (!mobileVerRespo.IsMobileVerified(customerMobile, customerEmail))
                    {
                        mobileVer = mobileVerificetion.ProcessMobileVerification(customerEmail, customerMobile);

                        SMSTypes st = new SMSTypes();
                        st.SMSMobileVerification(mobileVer.CustomerMobile, customerName, mobileVer.CWICode, HttpContext.Current.Request.ServerVariables["URL"].ToString());

                        isSuccess = true;
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
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                        IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                        isSuccess = objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
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

#if unused
        /// <summary>
        /// Written By : Ashwini Todkar on 3 Oct 2014
        /// PopulateWhere to set shecdule appointmnet date
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns>if a valid user then returns true else false</returns>
        [AjaxPro.AjaxMethod()]
        public bool UpdateAppointmentDate(UInt32 pqId, string date)
        {
            bool isSuccess = false;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isSuccess = objDealer.UpdateAppointmentDate(pqId, DateTime.Parse(date));

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
#endif
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isSuccess = objDealer.IsNewBikePQExists(pqId);
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isUpdated = objDealer.UpdatePQBikeColor(colorId, pqId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isDealerPricesAvailable = objDealer.IsDealerPriceAvailable(versionId, cityId);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

            PQOutputEntity objPQOutput = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    // save price quote
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId > 0 ? areaId : 0;
                    objPQEntity.ClientIP = CommonOpn.GetClientIP();
                    objPQEntity.SourceId = isMobileSource ? Convert.ToUInt16(Bikewale.Common.Configuration.MobileSourceId) : Convert.ToUInt16(Bikewale.Common.Configuration.SourceId);
                    objPQEntity.ModelId = modelId;

                    // If pqId exists then, set pqId
                    objPQOutput = objIPQ.ProcessPQ(objPQEntity);

                }
            }
            catch (Exception ex)
            {
                string selectedParams = "cityid : " + cityId + ", areaId : " + areaId + ", modelId : " + modelId;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " " + selectedParams);
                objErr.SendMail();
            }

            // Check if dealer price quote exists for the given area id
            finally
            {

                if (objPQOutput.PQId > 0)
                {
                    // Save pq cookie
                    PriceQuoteCookie.SavePQCookie(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString());
                }
            }

            response = "{\"quoteId\":\"" + objPQOutput.PQId + "\",\"dealerId\":\"" + objPQOutput.DealerId + "\"}";

            return response;
        }

    }   //End of Class
}   //End of namespace