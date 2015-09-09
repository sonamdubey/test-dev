using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.UI.Customer.Entities;
using Newtonsoft.Json;

namespace Bikewale.UI.Entities.Customer
{    
    public class AuthenticatedCustomer : CustomerBase
    {       
        public bool IsAuthorized { get; set; }        
        public string AuthenticationTicket { get; set; }
    }
}
