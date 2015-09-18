using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.UI.Entities.Customer
{
    public class RegisteredCustomer : AuthenticatedCustomer 
    {
        public bool IsNewCustomer { get; set; }        
    }
}
