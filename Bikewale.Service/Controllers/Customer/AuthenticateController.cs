using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Bikewale.DTO.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3 Sept 2015
    /// Summary : Controller have methods to authenticate the user.
    /// </summary>
    [Route("api/customer/authenticate/")]
    public class AuthenticateController : ApiController
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
        /// Function will authenticate the customer.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createAuthTicket"></param>
        /// <returns>Returns authenticated users basic details.</returns>
        [ResponseType(typeof(AuthenticatedCustomer))]
        public IHttpActionResult Get(string email, string password, bool? createAuthTicket = null)
        {
            CustomerEntity objCust = null;

            if(createAuthTicket.HasValue)
            {
                objCust = _authenticate.AuthenticateUser(email, password, createAuthTicket.Value);
            }           
            else
            {                
                objCust = _authenticate.AuthenticateUser(email, password);
            }

            AuthenticatedCustomer objCustomer = new AuthenticatedCustomer();

            try
            {
                if (objCust != null && objCust.IsExist == true)
                {                    
                    Mapper.CreateMap<CustomerEntity, AuthenticatedCustomer>();
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.AuthenticateController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }   //Get

    }   // class
}   // namespace
