using AutoMapper;
using Bikewale.BAL.Customer;
using Bikewale.DTO.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 7 Sept 2015
    /// Summary : Class have CRUD operations related to the customers
    /// </summary>
    public class CustomerController : ApiController
    {
        private readonly ICustomerRepository<CustomerEntity, UInt32> _customerRepository = null;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _authenticate = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="authenticate"></param>
        public CustomerController(ICustomerRepository<CustomerEntity, UInt32> customerRepository, ICustomerAuthentication<CustomerEntity, UInt32> authenticate)
        {
            _customerRepository = customerRepository;
            _authenticate = authenticate;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Sept 2015
        /// Summary : Function to register the customer with bikewale. All fields are mandatory
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Returns registered customers data.</returns>
        [ResponseType(typeof(RegisteredCustomer))]
        public IHttpActionResult POST([FromBody] RegisterInputParameters userInfo)
        {
            try
            {
                CustomerEntity objCust = _customerRepository.GetByEmail(userInfo.Email);
                RegisteredCustomer objRegCust = new RegisteredCustomer();


                if (objCust != null && !objCust.IsExist)
                {
                    CustomerEntity objinputCust = new CustomerEntity() { CustomerName = userInfo.Name, CustomerEmail = userInfo.Email, CustomerMobile = userInfo.Mobile, Password = userInfo.Password, ClientIP = userInfo.ClientIP };

                    // Generate hash and salt for the password.
                    RegisterCustomer objReg = new RegisterCustomer();

                    objinputCust.PasswordSalt = objReg.GenerateRandomSalt();
                    objinputCust.PasswordHash = objReg.GenerateHashCode(objinputCust.Password, objinputCust.PasswordSalt);

                    objRegCust.CustomerId = _customerRepository.Add(objinputCust);
                    objRegCust.CustomerEmail = userInfo.Email;
                    objRegCust.CustomerName = userInfo.Name;
                    objRegCust.CustomerMobile = userInfo.Mobile;
                    objRegCust.IsNewCustomer = true;

                    string authTicket = _authenticate.GenerateAuthenticationToken(objRegCust.CustomerId.ToString(), objRegCust.CustomerName, objRegCust.CustomerEmail);

                    objRegCust.AuthenticationTicket = authTicket;
                    objRegCust.IsAuthorized = true;

                    return Ok(objRegCust);
                }
                else
                {
                    Mapper.CreateMap<CustomerEntity, RegisteredCustomer>();
                    Mapper.CreateMap<CustomerEntityBase, CustomerBase>();
                    objRegCust = Mapper.Map<CustomerEntity, RegisteredCustomer>(objCust);
                    objRegCust.IsNewCustomer = false;

                    return Ok(objRegCust);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.CustomerController.POST");

                return InternalServerError();
            }
        }   // POST

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Dec 2017
        /// Description :   Function to logout a customer
        /// </summary>
        /// <param name="ReturnUrl"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        [HttpPost, Route("api/customer/logout/")]
        public IHttpActionResult Logout(string ReturnUrl, string hash = "")
        {
            if (Request.Headers.Contains("token") && Request.Headers.Contains("customerId"))
            {
                var token = Request.Headers.GetValues("token").First();
                var customerId = Request.Headers.GetValues("customerId").First();
                if (Bikewale.Utility.BikewaleSecurity.Decrypt(token).Equals(customerId))
                {
                    CurrentUser.EndSession();
                    string returnUrl = ReturnUrl;
                    if (!string.IsNullOrEmpty(hash))
                        returnUrl = returnUrl.Replace(Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, "");
                    if (IsLocalUrl(returnUrl))
                    {
                        if (!string.IsNullOrEmpty(hash))
                        {
                            returnUrl = string.Format("{0}#{1}", returnUrl, hash);
                        }
                        return Ok(returnUrl);
                    }
                    else
                    {
                        return Ok("/");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 09 Nov 2016
        /// Desc: Function to check if url is local url or not
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsLocalUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return false;
                }
                else
                {
                    return ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "IsLocalUrl()");

                return false;
            }
        }

    }   // Class
}   // namespace
