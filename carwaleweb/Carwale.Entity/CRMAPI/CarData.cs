using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CRMAPI
{
    public class CarData
    {
        public int CarVersionId { get; set; }
        public int CityId { get; set; }
        public ulong PQId { get; set; }
        public DateTime ExpectedBuyingDate { get; set; }
    }
}
