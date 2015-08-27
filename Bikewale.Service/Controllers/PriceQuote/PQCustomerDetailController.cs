using AutoMapper;
using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.Customer;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Detail Controller
    /// Author  :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// </summary>
    public class PQCustomerDetailController : ApiController
    {
        /// <summary>
        /// Saves the Customer details if it is a new customer.
        /// generated the OTP for the non verified customer
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQCustomerDetailOutput))]
        public HttpResponseMessage Post([FromBody]PQCustomerDetailInput input)
        {
            PQCustomerDetailOutput output = null;
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

                    if (!objAuthCustomer.IsRegisteredUser(input.CustomerEmail))
                    {
                        container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                        ICustomer<CustomerEntity, UInt32> objCustomer = container.Resolve<ICustomer<CustomerEntity, UInt32>>();

                        RegisterCustomer rc = new RegisterCustomer();
                        password = rc.GenerateRandomPassword();
                        salt = rc.GenerateRandomSalt();
                        hash = rc.GenerateHashCode(password, salt);

                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, PasswordSalt = salt, PasswordHash = hash, ClientIP = input.ClientIP };
                        UInt32 CustomerId = objCustomer.Add(objCust);
                    }

                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isSuccess = objDealer.SaveCustomerDetail(input.DealerId, input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail);

                    //DealerPriceQuoteCookie.CreateDealerPriceQuoteCookie(PriceQuoteCookie.PQId, false, false);
                    //CustomerDetailCookie.CreateCustomerDetailCookie(customerName, customerEmail, customerMobile);

                    container.RegisterType<IMobileVerificationRepository, Bikewale.BAL.MobileVerification.MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    if (!mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                    {
                        container.RegisterType<IMobileVerification, Bikewale.BAL.MobileVerification.MobileVerification>();
                        IMobileVerification mobileVerificetion = container.Resolve<IMobileVerification>();

                        mobileVer = mobileVerificetion.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);
                        isVerified = false;

                        SMSTypes st = new SMSTypes();
                        st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.PageUrl);
                    }
                    else
                    {
                        isVerified = objDealer.UpdateIsMobileVerified(input.PQId);

                        // If customer is mobile verified push lead to autobiz
                        if (isVerified)
                        {
                            AutoBizAdaptor.PushInquiryInAB(input.DealerId.ToString(), input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId, input.CityId);
                        }
                    }
                }
                if (isVerified)
                {
                    output = new PQCustomerDetailOutput();
                    output.IsSuccess = isVerified;
                    return Request.CreateResponse(HttpStatusCode.Created, output);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Not Modified");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }            
        }

        /// <summary>
        /// Gets the customer details for a generated price quote
        /// </summary>
        /// <param name="pqId">Price quote</param>
        /// <returns>Customer Details</returns>
        [ResponseType(typeof(PQCustomer))]
        public HttpResponseMessage Get(uint pqId)
        {
            PQCustomerDetail entity = null;
            PQCustomer output = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    entity = objDealer.GetCustomerDetails(pqId);
                }
                if (entity != null)
                {
                    output = PriceQuoteEntityToCTO.ConvertCustomerDetail(entity);
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
