using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserProfile
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : Enum to hold User Profile type.
    /// </summary>
    public enum UserProfileKeysEnum
    {
        #region the order of this list should be in sync with the service call in `Bikewale.BAL.ApiGateway.Adapters.Bhrigu.GetUserProfileAdapter.BuildRequest()`
        BikeType,
        ModelsList,
        BodyStyleList,
        BikeCount
        #endregion
    }
}
