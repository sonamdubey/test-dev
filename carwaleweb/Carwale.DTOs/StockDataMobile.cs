using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity;
using Carwale.Entity.Classified;

namespace Carwale.DTOs
{
    public class StockDataMobile
    {
        public List<StockBaseEntityMobile> ListingData { get; set; }
        public PagerOutputEntity PagerData { get; set; }
    }
}
