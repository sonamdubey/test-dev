using Carwale.Entity.Classified.Search;
using System.Collections.Generic;

namespace Carwale.UI.ViewModels.Used.Dealers
{
    public class VirtualPageViewModel
    {
        public IList<StockBaseData> Results { get; set; }
        public string DealerLogo { get; set; }
        public string DealerName { get; set; }
        public string Title { get; set; }
        public int FirstFoldStocksCount { get; } = 6;
    }
}