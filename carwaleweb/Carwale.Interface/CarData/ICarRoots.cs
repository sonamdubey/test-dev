using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CarData
{
    public interface ICarRoots
    {        
        List<int> GetYearsByRootId(int rootId);
    }
}
