using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CRMAPI
{
    public class CustomerData
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; } 
        public int Source { get; set; }
        public int LeadSourceCategoryId { get; set; }
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }
        public ulong CWCustId { get; set; }
    }
}
