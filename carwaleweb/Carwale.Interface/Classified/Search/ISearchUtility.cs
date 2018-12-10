using Carwale.Entity.Classified.Search;
using System.Collections.Specialized;

namespace Carwale.Interfaces.Classified.Search
{
    public interface ISearchUtility
    {
        string GetURL(string makeName, string rootName, string cityName, int pageNo = 0, string queryString = null);
        NameValueCollection RemoveParamsFromQs(string qs, string[] keys);
    }
}
