using Bikewale.UI.Customer.Entities;

namespace Bikewale.UI.Entities.Customer
{
    public class AuthenticatedCustomer : CustomerBase
    {       
        public bool IsAuthorized { get; set; }        
        public string AuthenticationTicket { get; set; }
    }
}
