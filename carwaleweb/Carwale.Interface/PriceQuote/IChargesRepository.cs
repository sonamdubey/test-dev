using Carwale.Entity.Price;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IChargesRepository
    {
        Dictionary<int, Charge> GetCharges();
        List<int> GetComponents(int chargeId);
    }
}
