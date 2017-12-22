using System.Collections.Generic;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : For caching of dealers list w.r.t. city and make. 
    /// Modified by :   Sumit Kate on 19 Jun 2016
    /// Descrption  :   Added optional parameter modelId for GetDealerByMakeCity
    /// Modified by  :   Sumit Kate on 21 Jun 2016
    /// Description :   Get Popular City Dealer Count.
    /// Modified by  :   Subodh jain on 20 Dec 2016
    /// Description :   Get Dealer By BrandList
    /// Modified by :  Subodh Jain on 21 Dec 2016
    /// Description :   Merge Dealer and service center for make and model page
    /// Modified by : Sajal Gupta on 19-12-216
    /// Desc : Added function FetchNearByCityDealersCount;
    /// </summary>
    public interface IDealerCacheRepository
    {
        DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelid = 0);
        DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId);
        DealerBikesEntity GetDealerDetailsAndBikesByDealerAndMake(uint dealerId, int makeId);
        PopularDealerServiceCenter GetPopularCityDealer(uint makeId, uint topCount);
        IEnumerable<NewBikeDealersMakeEntity> GetDealersMakesList();
        IEnumerable<DealerBrandEntity> GetDealerByBrandList();
        IEnumerable<NearByCityDealerCountEntity> FetchNearByCityDealersCount(uint makeId, uint cityId);
        IEnumerable<CityEntityBase> FetchDealerCitiesByMake(uint makeId);
        DealerBikeModelsEntity GetBikesByDealerAndMake(uint dealerId, uint makeId);
    }
}
