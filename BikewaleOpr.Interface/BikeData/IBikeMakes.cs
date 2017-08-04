using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar
    /// Description : Interface for bike makes
    /// </summary>
    public interface IBikeMakes
    {
        IEnumerable<BikeModelEntityBase> GetModelsByMake(uint makeId);
        IEnumerable<BikeMakeEntityBase> GetServiceCenterMakes(ushort RequestType);
      
    }
}
