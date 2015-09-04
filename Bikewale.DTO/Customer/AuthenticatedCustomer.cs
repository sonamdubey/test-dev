using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Customer
{
    public class AuthenticatedCustomer : CustomerBase
    {
        [JsonProperty("isAuthorized")]
        public bool IsAuthorized { get; set; }

        [JsonProperty("authenticationTicket")]
        public string AuthenticationTicket { get; set; }
    }
}
