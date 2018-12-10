using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Finance
{
    public class FinanceUrlParameter
    {
        public string HostUrl { get; set; }
        public string ProfileId { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int MakeYear { get; set; }
        public int CityId { get; set; }
        public int PriceNumeric { get; set; }
        public short? OwnerNumeric { get; set; }
        public int MakeMonth { get; set; }
    }
}
