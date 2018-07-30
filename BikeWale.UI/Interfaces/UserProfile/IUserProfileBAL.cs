using Bikewale.Entities.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.UserProfile
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : Interface for `UserProfileBAL`
    /// </summary>
    public interface IUserProfileBAL
    {
        UserProfileTargeting GetUserProfile(string userCookie);
    }
}
