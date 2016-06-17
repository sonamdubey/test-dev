using Bikewale.Entities.DealerLocator;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : For caching of dealers list w.r.t. city and make. 
    /// Modified by :   Sumit Kate on 19 Jun 2016
    /// Descrption  :   Added optional parameter modelId for GetDealerByMakeCity
    /// </summary>
    public interface IDealerCacheRepository
    {
        DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelId = 0);
        DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId);
    }
}
