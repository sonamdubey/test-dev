

using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
	/// <summary>
	/// Created by  : Pratibha Verma on 9 May 2018
	/// Description : Interface for BikeModel Cache Helper
	/// </summary>
    public interface IBikeModelsCacheHelper
    {
        IEnumerable<MakeModelListEntity> GetMakeModelList(EnumBikeType requestType);
    }
}
