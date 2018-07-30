using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Vivek Gupta 
    /// Date : 24 june 2016
    /// desc : GetDealerStates reference added 
    /// </summary>
    public interface IState
    {
        List<StateEntityBase> GetStates();
        IEnumerable<DealerStateEntity> GetDealerStates(uint makeId);
        DealerLocatorList GetDealerStatesCities(uint makeId);
        Hashtable GetMaskingNames();
    }
}
