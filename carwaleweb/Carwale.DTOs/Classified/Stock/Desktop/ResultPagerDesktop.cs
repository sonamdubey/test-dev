using Carwale.Entity;
using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class ResultPagerDesktop
    {
        public IList<StockBaseEntity> ResultsData { get; set; }

        public PagerOutputEntity PagerData { get; set; }
    }
}
