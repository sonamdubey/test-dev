
using Carwale.Entity;
namespace Carwale.Interfaces.PriceQuote
{
    public interface IPricesCacheRepository<T1, T2>
    {
        CarEntity GetVersionModel(int versionId);
        bool InvalidateCache(T1 pricesInput);
    }
}
