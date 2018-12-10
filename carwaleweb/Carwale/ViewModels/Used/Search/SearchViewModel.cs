using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Classified.Search;
using System.Collections.Generic;

namespace Carwale.UI.ViewModels.Used.Search
{
    public class SearchViewModel
    {
        public IList<StockBaseData> StockList { get; set; }
        public SearchParams SearchParams { get; set; }
        public string FilterCityName { get; set; }
        public int FilterCityId { get; set; }
        public int TotalStockCount { get; set; }
        public bool IsSimilarCarAvailable { get; set; }
        public string NextPageUrl { get; set; }
        public IEnumerable<RootBase> RootsInfo { get; set; }
        public IEnumerable<MakeEntity> MakesInfo { get; set; }
        public string RootsInfoInJson { get; set; }
        public string MakesInfoInJson { get; set; }
        public AdParams AdUnit { get; set; }
        public MetaKeywords MetaKeywords { get; set; }
        public bool IsSoldOut { get; set; }
        public int StockFetched { get; set; }
        public string CurrQS { get; set; }
        public string SortBaseUrlAmp { get; set; }
        public NearbyCarsBucket LastNearbyCarsBucket { get; set; }
        public string CarsNearMeLabel { get; set; }
        public bool IsCustomerAreaAvailable { get; set; }
        public string CustAreaName { get; set; }
        public bool ShouldFetchNearbyCars { get; set; }
    }
}
