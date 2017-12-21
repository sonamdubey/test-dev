using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar
    /// Description : Interface for bike makes
    /// </summary>
    public interface IBikeMakes
    {
        IEnumerable<BikeModelEntityBase> GetModelsByMake(EnumBikeType requestType, uint makeId);
        IEnumerable<BikeMakeEntityBase> GetMakes(ushort requestType);
        BikeMakeEntity GetMakeDetailsById(uint makeId);
    }
}
