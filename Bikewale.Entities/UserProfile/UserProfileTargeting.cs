using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserProfile
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : Entity to hold User Profile data.
    /// </summary>
    public class UserProfileTargeting
    {
        public IEnumerable<KeyValuePair<UserProfileKeysEnum, string>> TargetingData { get; set; }
    }
}
