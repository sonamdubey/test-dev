using Carwale.Entity.Finance;

namespace Carwale.Interfaces.Finance
{
    public interface IFinanceLinkDataRepository
    {
        FinanceLinkData GetUrlData(int platformId, int screenId);
    }
}
