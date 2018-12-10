using Carwale.Entity.CrossSell;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CrossSell
{
    public interface IPaidCrossSell
    {
        List<CrossSellDetail> GetPaidCrossSellList(int versionId, Location locationObj);
        CrossSellDetail GetPaidCrossSell(int versionId, Location locationObj);
    }
}
