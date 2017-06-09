using Bikewale.DAL.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.Customer
{
    public class Customer<T, U> : ICustomer<T, U> where T : CustomerEntity, new()
    {
        private readonly ICustomerRepository<T, U> customerRepository = null;

        public Customer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICustomerRepository<T, U>, CustomerRepository<T, U>>();
                customerRepository = container.Resolve<ICustomerRepository<T, U>>();
            }
        }

        public T GetByEmail(string emailId)
        {
            T t = customerRepository.GetByEmail(emailId);

            return t;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 07 Jun 2017
        /// Description :   Returns customer by email or mobile
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public T GetByEmailMobile(string emailId, string mobile)
        {
            return customerRepository.GetByEmailMobile(emailId, mobile);
        }

        public U Add(T t)
        {
            // If password is not given by customer generate random password (In case of automate registration).
            // Else use customer given password.
            // Create salt and hash for the password.
            RegisterCustomer objCust = new RegisterCustomer();

            //dummy email is generated as <mobile>@unknown.com
            if (String.IsNullOrEmpty(t.CustomerEmail))
            {
                t.CustomerEmail = String.Format("{0}@unknown.com", t.CustomerMobile);
            }

            if (String.IsNullOrEmpty(t.Password))
            {
                t.Password = objCust.GenerateRandomPassword();
                t.PasswordSalt = objCust.GenerateRandomSalt();
                t.PasswordHash = objCust.GenerateHashCode(t.Password, t.PasswordSalt);
            }
            else
            {
                t.PasswordSalt = objCust.GenerateRandomSalt();
                t.PasswordHash = objCust.GenerateHashCode(t.Password, t.PasswordSalt);
            }

            U u = customerRepository.Add(t);

            return u;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 07 Jun 2017
        /// Description :   Call repo method
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(T t)
        {
            return customerRepository.Update(t);
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            List<T> tList = customerRepository.GetAll();

            return tList;
        }

        public T GetById(U id)
        {
            T t = customerRepository.GetById(id);

            return t;
        }
    }   // class
}   // namespace
