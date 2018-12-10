using Carwale.Entity.Classified.Leads;

namespace Carwale.Interfaces.Classified
{
    public interface IUsedCarBuyerCacheRepository
    {
        BuyerInfo GetBuyerInfo(string userId);
    }
}
