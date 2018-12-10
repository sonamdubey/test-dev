using Carwale.Entity.Classified.Leads;

namespace Carwale.Interfaces.Classified
{
    public interface IUsedCarBuyerRepository
    {
        BuyerInfo GetBuyerInfo(string userId);
    }
}
