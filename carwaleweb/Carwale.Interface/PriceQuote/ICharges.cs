using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface ICharges
    {
        List<ChargeBase> GetComponents(int chargeId);
    }
}