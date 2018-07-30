
using Bikewale.Entities.Finance.CapitalFirst;
using System.Collections.Generic;
namespace Bikewale.Interfaces.Finance
{
    /// <summary>
    /// Created by : Snehal Dange on 25th May 2018
    /// Description : Interface for finance .
    /// </summary>
    public interface IFinanceCacheRepository
    {
        IEnumerable<CityPanMapping> GetCapitalFirstPanCityMapping();
    }
}
