using AutoMapper;
using Bikewale.BAL.Customer;
using Bikewale.DTO.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;
using System;
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.CustomerController.POST");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // POST

    }   // Class
}   // namespace
