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
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealer = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerificetion = null;

        public PQCustomerDetailController(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealer,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealer = objDealer;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerificetion = mobileVerificetion;
        }
        /// <summary>
        /// Saves the Customer details if it is a new customer.
        /// generated the OTP for the non verified customer
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQCustomerDetailOutput))]
        public IHttpActionResult Post([FromBody]PQCustomerDetailInput input)
        {
            PQCustomerDetailOutput output = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            CustomerEntity objCust = null;
            MobileVerificationEntity mobileVer = null;

            try
            {
                if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail))
                {
                    RegisterCustomer rc = new RegisterCustomer();
                    password = rc.GenerateRandomPassword();
                    salt = rc.GenerateRandomSalt();
                    hash = rc.GenerateHashCode(password, salt);

                    objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, PasswordSalt = salt, PasswordHash = hash, ClientIP = input.ClientIP };
                    UInt32 CustomerId = _objCustomer.Add(objCust);
                }

                isSuccess = _objDealer.SaveCustomerDetail(input.DealerId, input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail);

                if (!_mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                {
                    mobileVer = _mobileVerificetion.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);
                    isVerified = false;

                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.PageUrl);
                }
                else
                {
                    isVerified = _objDealer.UpdateIsMobileVerified(input.PQId);
                    // If customer is mobile verified push lead to autobiz
                    if (isVerified)
                    {
                        AutoBizAdaptor.PushInquiryInAB(input.DealerId.ToString(), input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId, input.CityId);
                    }
                }
                if (isVerified)
                {
                    output = new PQCustomerDetailOutput();
                    output.IsSuccess = isVerified;
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Gets the customer details for a generated price quote
        /// </summary>
        /// <param name="pqId">Price quote</param>
        /// <returns>Customer Details</returns>
        [ResponseType(typeof(PQCustomer))]
        public IHttpActionResult Get(uint pqId)
        {
            PQCustomerDetail entity = null;
            PQCustomer output = null;
            try
            {
                entity = _objDealer.GetCustomerDetails(pqId);
                if (entity != null)
                {
                    output = PQCustomerMapper.Convert(entity);
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
