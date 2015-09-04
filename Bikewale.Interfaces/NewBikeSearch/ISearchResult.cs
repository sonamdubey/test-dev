using Bikewale.Entities.NewBikeSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface ISearchResult
    {
        SearchOutputEntity GetSearchResult(FilterInput filterInputs, InputBaseEntity input);
    }
}
