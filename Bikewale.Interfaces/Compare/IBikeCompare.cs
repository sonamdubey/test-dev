using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Compare
{
    public interface IBikeCompare
    {
        BikeCompareEntity DoCompare(string versions);
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
    }
}
