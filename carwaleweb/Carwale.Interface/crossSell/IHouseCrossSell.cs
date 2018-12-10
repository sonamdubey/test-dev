using Carwale.Entity.CrossSell;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CrossSell
{
    public interface IHouseCrossSell
    {
        List<CrossSellDetail> GetHouseCrossSellList(int versionId, Location locationObj, int platformId);
        CrossSellDetail GetHouseCrossSell(int versionId, Location locationObj, int platformId);
    }
}
