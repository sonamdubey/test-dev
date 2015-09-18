using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.DAL.Customer;
using System.Web.Security;
using Bikewale.Notifications;

namespace Bikewale.BAL.Customer
{
    /// <summary>
    /// Created y : Ashish G. Kamble on 25 apr 2014
    /// Summary : Class all functions related to the customer authentication and other related operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class CustomerAuthentication<T,U> : ICustomerAuthentication<T,U> where T : CustomerEntity, new()
    {
        private readonly ICustomerRepository<T, U> customerRepository = null;

        public CustomerAuthentication()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICustomerRepository<T, U>, CustomerRepository<T, U>>();
                customerRepository = container.Resolve<ICustomerRepository<T, U>>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsRegisteredUser(string email)
        {
            bool isRegistered = false;
            ICustomer<T, U> objCust = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICustomer<T, U>, Customer<T, U>>();
                objCust = container.Resolve<ICustomer<T, U>>();
            }

            CustomerEntity objCustEntity = objCust.GetByEmail(email);

            isRegistered = objCustEntity.IsExist;

            return isRegistered;
        }

        /// <summary>
        /// Written By : Ashish G. kamble on 8 Sept 2015
        /// Summary : Function to generate the authentication ticket
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="custName"></param>
        /// <param name="custEmail"></param>
        /// <returns>Returns authenticated ticket</returns>
        public string GenerateAuthenticationToken(string custId, string custName, string custEmail)
        {
            string authTicket = string.Empty;

            try
            {
                //create a ticket and add it to the cookie
                FormsAuthenticationTicket ticket;
                //now add the id and the role to the ticket, concat the id and role, separated by ',' 
                ticket = new FormsAuthenticationTicket(
                            1,
                            custName,
                            DateTime.Now,
                            DateTime.Now.AddDays(365),
                            false,
                            custId + ":" + custEmail
                        );

                authTicket = FormsAuthentication.Encrypt(ticket);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GenerateAuthenticationToken");
                objErr.SendMail();
            }

            return authTicket;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Sept 2015
        /// Summary : Function to authenticate the customer with bikewale database. authentication ticket creation is optional
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createAuthTicket"></param>
        /// <returns></returns>
        public T AuthenticateUser(string email, string password, bool? createAuthTicket = null)
        {
            ICustomer<T, U> objCust = null;
            T objCustEntity = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICustomer<T, U>, Customer<T, U>>();
                objCust = container.Resolve<ICustomer<T, U>>();
            }

            objCustEntity = objCust.GetByEmail(email);

            if(objCustEntity != null)
            { 
                RegisterCustomer objRegister = new RegisterCustomer();
                string userHash = objRegister.GenerateHashCode(password, objCustEntity.PasswordSalt);

                if (string.Equals(userHash, objCustEntity.PasswordHash))
                {
                    if (createAuthTicket.HasValue)
                    {
                        objCustEntity.AuthenticationTicket = GenerateAuthenticationToken(objCustEntity.CustomerId.ToString(), objCustEntity.CustomerName, objCustEntity.CustomerEmail);
                    }
                }
                else
                {
                    objCustEntity = null;
                }
            }

            return objCustEntity;
        }

        public T AuthenticateUser(string email)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomerMobileNumber(string mobile, string email, string name = null)
        {
            customerRepository.UpdateCustomerMobileNumber(mobile, email, name);
        }

        public void UpdatePasswordSaltHash(U customerId, string passwordSalt, string passwordHash)
        {
            customerRepository.UpdatePasswordSaltHash(customerId, passwordHash, passwordHash);
        }

        public void SavePasswordRecoveryToken(U customerId, string token)
        {
            customerRepository.SavePasswordRecoveryToken(customerId, token);
        }

        public bool IsValidPasswordRecoveryToken(U customerId, string token)
        {
            bool isValidToken = false;

            isValidToken = customerRepository.IsValidPasswordRecoveryToken(customerId, token);

            return isValidToken;
        }

        public void DeactivatePasswordRecoveryToken(U customerId)
        {
            customerRepository.DeactivatePasswordRecoveryToken(customerId);
        }

    }
}
