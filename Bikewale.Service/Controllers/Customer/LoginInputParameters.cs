using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Service.Controllers.Customer
{
    public class LoginInputParameters
    {
       public string Email {get;set;}
       public string Password {get;set;}
       public bool? CreateAuthTicket { get; set; }
    }
}
