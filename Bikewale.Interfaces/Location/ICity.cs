using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System.Collections;
using System.Collections.Generic;

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
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
    }
}
