using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPricesRepository<T1, T2>
    {
        bool InsertPriceQuote(List<T2> pricesInput, int cityId, int updatedBy, ref List<int> PriceAddedVersions);
        bool DeletePriceQuote(List<T2> pricesInput, int cityId, int updatedBy, ref List<int> PriceDeletedVersions);
    }
}
