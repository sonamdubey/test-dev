using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Used.Search;

namespace Bikewale.Interfaces.Used.Search
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 11 sept 2016
    /// </summary>
    public interface ISearch
    {
        SearchResult GetUsedBikesList(InputFilters inputFilters);
    }
}
