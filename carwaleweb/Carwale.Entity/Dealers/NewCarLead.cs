using Carwale.Entity.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    public class NewCarLead : CustomerMinimal
    {
        public int DealerId { get; set; }
        public string DealerEmail { get; set; }
        public string DealerName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int CityId { get; set; }
        public DateTime RequestDate { get; set; }
        public int LeadRequestType { get; set; }
        public int VersionId { get; set; }
        public int inquirySourceId { get; set; }
        public string Comments { get; set; }
        public int PlatformId { get; set; }
    }
}
