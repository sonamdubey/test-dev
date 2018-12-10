using Carwale.Entity.Elastic;
using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.PriceQuote
{
    public interface INearbyCitiesSearch
    {
        bool AddToIndex(int versionId, int cityId);
        bool DeleteFromIndex(int versionId, int cityId);
        List<VersionCityPricesObj> GetNearByCities(int versionId, int cityId, int count);
        void AddToIndex(List<VehiclePrice> vehiclePriceList);
    }
}
