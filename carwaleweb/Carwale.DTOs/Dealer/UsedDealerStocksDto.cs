using Carwale.DTOs.Stock.SimiliarCars;
using System.Collections.Generic;

namespace Carwale.DTOs.Dealer
{
    public class UsedDealerStocksDto
    {
        public IEnumerable<StockSummaryDTO> Stocks { get; set; }
        public string NextPageUrl { get; set; }
    }
}
