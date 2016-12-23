
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike models 
    /// </summary>
    public interface IBikeModels
    {
        IEnumerable<BikeModelEntityBase> GetModels(uint makeId, string requestType);
    }
}
