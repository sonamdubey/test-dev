using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IChargeGroupsRepository
    {
        Dictionary<int, ChargeGroup> GetChargeGroups();
    }
}
