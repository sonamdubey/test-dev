
using Carwale.Entity.Classified.CarValuation;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ISellCarCacheRepository
    {
        void StoreBuyingIndex(int inquiryId, int buyingIndex);
        int GetBuyingIndex(int inquiryId, ValuationUrlParameters valuationUrlParameters = null);
        IEnumerable<int> C2BCities();
    }
}
