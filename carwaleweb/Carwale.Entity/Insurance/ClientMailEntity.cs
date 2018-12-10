using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance
{
    public class ClientMailEntity
    {
        public CarNameEntity Car { get; set; }
        public CustomersBasicInfo Customer { get; set; }
        public string InsuranceType { get; set; }
        public string RegistrationDate { get; set; }
        public string ClientEmails { get; set; }
        public int LeadId { get; set; }
    }
}
