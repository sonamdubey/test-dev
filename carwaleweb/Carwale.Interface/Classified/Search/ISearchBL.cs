using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Stock.Search;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Carwale.Interfaces.Classified.Search
{
    public interface ISearchBL
    {
        SearchResultMobile FetchData(SearchParams searchParams, Platform source, bool isAjaxRequest, string queryString, out string redirectUrl);
        IEnumerable<Cities> GetCities();
        AdParams GetAdUnit(string budget, string car, IEnumerable<MakeEntity> makesInfo, IEnumerable<RootBase> rootsInfo);
        bool CheckSimilarCarsAvailability(int totalStockCount, int cityId);
        MetaKeywords GetMetaKeyWords(SearchParams searchParams, int totalStockCount, Platform source);
        string GetCurrentPageQS(string qs, SearchParams searchParams);
        IEnumerable<RootBase> GetRootsName(string car);
        IEnumerable<MakeEntity> GetMakesName(string car);
        string GetRedirectQsByModelIds(NameValueCollection qs);
        int GetStocksCountByField(SearchParams searchParams, string field, double fieldValue, bool greaterThanFieldValue);
        IEnumerable<CarMakeEntityBase> GetCarMakes();
    }
}
