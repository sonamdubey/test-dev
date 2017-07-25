
using BikeWale.Entities.AutoBiz;

namespace Bikewale.Interfaces.AutoBiz
{
    /// <summary>
    /// Modified By :   Sumit Kate on 21 Mar 2015
    /// Description :   New IsSubscribedDealerExists to get dealer for bike version and area
    /// Modified by :   Sumit Kate on 18 Jul 2017
    /// Description :   Add new method GetSubscriptionDealer for primary dealer allocation
    /// </summary>
    public interface IDealer
    {
        DealerInfo GetSubscriptionDealer(uint modelId, uint cityId, uint areaId);
    }
}
