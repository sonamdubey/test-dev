using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.SimiliarCars
{
    public class SimilarStocksApp
    {
        public string HostUrl { get; set; }
        public IList<StockSummaryApp> Stocks { get; set; }
    }
}
