
using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike versions 
    /// </summary>
    public interface IBikeVersions
    {
        IEnumerable<BikeVersionEntityBase> GetVersions(uint modelId, string requestType);
    }
}
