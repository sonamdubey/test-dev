using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// Modified by :   Sumit Kate on 22 Mar 2016
    /// Description :   Added new function FetchDealerCitiesByMake    
    /// Modified By : Lucky Rathore on 23 March 2016
    /// Description : DealerBikesEntity GetDealerBikes(UInt16 dealerId);  added, used in DealerLocator.
    /// Description : DealerLocatorEntity GetDealerByMakeCity(uint cityId, uint makeId);  added, used in DealerLocator.
    /// Modified by :   Sumit Kate on 19 Jun 2016
    /// Descrption  :   Added optional parameter modelId for GetDealerByMakeCity
    /// Modified by  :   Sumit Kate on 21 Jun 2016
    /// Description :   Get Popular City Dealer Count.
    /// Modified by : Sajal Gupta on 19-12-2016
    /// Desc : Added  FetchNearByCityDealersCount function
    /// Modified by  :   Subodh jain on 20 Dec 2016
    /// Description :   Get Dealer By BrandList
    /// Modified by :  Subodh Jain on 21 Dec 2016
    /// Description :   Merge Dealer and service center for make and model page
    /// </summary>    
    public interface IDealerRepository
    {
        List<NewBikeDealersMakeEntity> GetDealersMakesList();
        NewBikeDealersListEntity GetDealersCitiesListByMakeId(uint makeId);
        IEnumerable<CityEntityBase> FetchDealerCitiesByMake(uint makeId);
        List<NewBikeDealerEntity> GetDealersList(uint makeId, uint cityId);
        List<BikeMakeEntityBase> GetDealersMakeListByCityId(uint cityId);
        List<CityEntityBase> GetDealersCitiesList();
        List<CityEntityBase> GetDealersBookingCitiesList();
        //Added by sushil kumar on 7th Oct 2015
        IEnumerable<NewBikeDealerEntityBase> GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null);
        bool SaveManufacturerLead(ManufacturerLeadEntity customer);
        DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelid = 0);
        DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId);
        DealerBikesEntity GetDealerDetailsAndBikesByDealerAndMake(uint dealerId, int makeId);
        PopularDealerServiceCenter GetPopularCityDealer(uint makeId, uint topCount);
        bool UpdateManufaturerLead(uint pqId, string custEmail, string mobile, string response);
        IEnumerable<DealerBrandEntity> GetDealerByBrandList();
        IEnumerable<NearByCityDealerCountEntity> FetchNearByCityDealersCount(uint makeId, uint cityId);
        DealerBikeModelsEntity GetBikesByDealerAndMake(uint dealerId, uint makeId);
    }
}
