using Carwale.Entity.Finance;

namespace Carwale.Interfaces.Finance
{
    public interface IFinanceLinkDataCache
    {
        FinanceLinkData GetUrlData(int platformId, int screenId);
    }
}
