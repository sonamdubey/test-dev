using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface ISearchQuery
    {
        string GetSelectClause();
        string GetFromClause();
        string GetWhereClause();
        string GetOrderByClause();
        string GetRecordCountQry();
        string GetSearchResultQuery();
        void InitSearchCriteria(FilterInput filterInputs);
    }
}
