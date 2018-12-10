using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.SimiliarCars
{
    public class SimilarStocksDTO
    {
        public string HostUrl { get; set; }
        public IList<StockSummaryDTO> Stocks { get; set; }
    }
}
