using Bikewale.Entities.NewBikeSearch;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IProcessFilter
    {
        FilterInput ProcessFilters(InputBaseEntity objInput);
    }
}
