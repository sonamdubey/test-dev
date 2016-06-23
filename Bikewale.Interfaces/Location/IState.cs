using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    public interface IState
    {
        List<StateEntityBase> GetStates();
        IEnumerable<DealerStateEntity> GetDealerStates(uint makeId);
    }
}
