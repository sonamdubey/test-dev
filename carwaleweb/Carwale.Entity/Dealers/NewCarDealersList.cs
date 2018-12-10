using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class NewCarDealersList
    {
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerArea { get; set; }
        public string DealerCode { get; set; }
        public int ClientId { get; set; }
    }
}
