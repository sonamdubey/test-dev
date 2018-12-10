using Carwale.Entity.Price;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IChargeGroupsCacheRepository
    {
        Dictionary<int, ChargeGroup> GetChargeGroups();
    }
}
