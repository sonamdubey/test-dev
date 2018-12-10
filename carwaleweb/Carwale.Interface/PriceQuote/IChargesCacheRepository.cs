using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IChargesCacheRepository
    {
        Dictionary<int, Charge> GetCharges();
        List<int> GetComponents(int chargeId);
    }
}
