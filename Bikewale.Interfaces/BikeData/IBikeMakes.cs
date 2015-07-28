using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike makes data.
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeMakes<T, U> : IRepository<T, U>
    {
        List<BikeMakeEntityBase> GetMakesByType(EnumBikeType requestType);
        List<BikeModelsListEntity> GetModelsList(U makeId);       
        BikeDescriptionEntity GetMakeDescription(U makeId);
    }
}