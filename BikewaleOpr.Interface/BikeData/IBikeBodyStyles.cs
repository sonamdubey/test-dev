using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Rajan Chauhan on 12th Dec 2017
    /// Description : Interface for bike series body style
    /// </summary>
    public interface IBikeBodyStyles
    {
        IEnumerable<BikeBodyStyleEntity> GetBodyStylesList();
    }
}
