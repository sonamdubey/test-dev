using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Modified By : Ashish G. Kamble on 7 June 2016
    /// </summary>
    public interface ICity
    {
        List<CityEntityBase> GetPriceQuoteCities(uint modelId);
        List<CityEntityBase> GetAllCities(EnumBikeType requestType);
        List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType);
        Hashtable GetMaskingNames();
        Hashtable GetOldMaskingNames();
    }
}
