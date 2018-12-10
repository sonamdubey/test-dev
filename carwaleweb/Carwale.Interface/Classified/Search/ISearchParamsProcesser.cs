
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.Classified.Search
{
    public interface ISearchParamsProcesser
    {
        void ProcessSearchParams(SearchParams searchParams, Platform source, bool isAjaxRequest, out string redirectUrl);
    }
}
