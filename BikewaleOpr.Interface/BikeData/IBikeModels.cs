
using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    public interface IBikeModels
    {
        IEnumerable<UsedModelsByMake> GetPendingUsedBikesWithoutModelImage();
    }
}
