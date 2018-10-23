using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
	/// <summary>
	/// Created By : Ashish G. Kamble
	/// Modified By : Ashish G. Kamble on 7 June 2016
	/// Modified By Vive Gupta on 24 june 2016
	/// Desc : added description for GetDealerStateCities
	/// Modified By:-Subodh Jain 29 dec 2016
	/// Summary :- Get Used Bike By Make City With Count
	/// Modified By : Pratibha Verma on 17 May 2018
	/// Description : Added GetModelPriceCities method
	/// </summary>
	public interface ICity
    {
        List<CityEntityBase> GetPriceQuoteCities(uint modelId);
        IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType);
        List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType);
        Hashtable GetMaskingNames();
        Hashtable GetOldMaskingNames();
        DealerStateCities GetDealerStateCities(uint makeId, uint stateId);
        IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount();
        IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeid);
		IEnumerable<CityEntityBase> GetModelPriceCities(uint modelId, byte popularCityCount);
        CityPriceEntity GetCityInfoByCityId(uint cityId);


    }
}
