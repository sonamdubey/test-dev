using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.DAL.Customer;

namespace Bikewale.BAL.Customer
{
    public class Customer<T,U> : ICustomer<T,U> where T : CustomerEntity, new()
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

        public U Add(T t)
        {
            U u = customerRepository.Add(t);

            return u;
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
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
