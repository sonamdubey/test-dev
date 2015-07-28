using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Customer;

namespace Bikewale.Interfaces.Customer
{
    /// <summary>
    /// created By : Ashish G. Kamble on 25 Apr 2014
    /// Summary : Interface to have basic functionality of the customer. Can be consumed on BAL.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface ICustomer<T,U> : IRepository<T,U> 
    {
        T GetByEmail(string emailId);
    }   
}
