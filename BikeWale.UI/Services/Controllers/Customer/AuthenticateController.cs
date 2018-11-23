using AutoMapper;
using Bikewale.DTO.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3 Sept 2015
    /// Summary : Controller have methods to authenticate the user.
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    [Route("api/customer/authenticate/")]
    public class AuthenticateController : CompressionApiController//ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _authenticate = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticate"></param>
        public AuthenticateController(ICustomerAuthentication<CustomerEntity, UInt32> authenticate)
        {
            _authenticate = authenticate;
        }

        /// <summary>
        ///  Function will authenticate the customer.
        ///  Modified By : Sushil Kumar on 25th July 2016
        ///  Description : Added check for null and empty email id and password
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns>Returns authenticated users basic details.</returns>
        [ResponseType(typeof(AuthenticatedCustomer))]
        public IHttpActionResult POST(LoginInputParameters objLogin)
        {
            CustomerEntity objCust = null;
            try
            {

                if (objLogin != null && !String.IsNullOrEmpty(objLogin.Email) && !String.IsNullOrEmpty(objLogin.Password))
                {
                    if (objLogin.CreateAuthTicket.HasValue)
                    {
                        objCust = _authenticate.AuthenticateUser(objLogin.Email, objLogin.Password, objLogin.CreateAuthTicket.Value);
                    }
                    else
                    {
                        objCust = _authenticate.AuthenticateUser(objLogin.Email, objLogin.Password);
                    }

                    AuthenticatedCustomer objCustomer = new AuthenticatedCustomer();

                    if (objCust != null && objCust.IsExist == true)
                    {
                     
                        objCustomer = Mapper.Map<CustomerEntity, AuthenticatedCustomer>(objCust);

                        objCustomer.IsAuthorized = true;

                        return Ok(objCustomer);
                    }
                    else
                    {
                        objCustomer.IsAuthorized = false;
                        return Ok(objCustomer);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.AuthenticateController.Get");
               
                return InternalServerError();
            }
        }   //Get

    }   // class
}   // namespace
