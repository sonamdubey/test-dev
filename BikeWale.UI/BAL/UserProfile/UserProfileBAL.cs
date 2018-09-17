using Bikewale.BAL.ApiGateway.Adapters.Bhrigu;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.UserProfile;
using Bikewale.Interfaces.UserProfile;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.UserProfile
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : BAL for User Profiling
    /// </summary>
    public class UserProfileBAL : IUserProfileBAL
    {
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public UserProfileBAL(IApiGatewayCaller apiGatewayCaller)
        {
            _apiGatewayCaller = apiGatewayCaller;
        }
        public UserProfileTargeting GetUserProfile(string userCookie)
        {
            if (string.IsNullOrEmpty(userCookie))
            {
                return null;
            }

            UserProfileTargeting profileTargeting = null;
            try
            {
                GetUserProfileAdapter userProfileAdapter = new GetUserProfileAdapter();
                userProfileAdapter.AddApiGatewayCall(_apiGatewayCaller, userCookie);
                _apiGatewayCaller.Call();
                if(userProfileAdapter.Output != null)
                {
                    profileTargeting = new UserProfileTargeting();
                    profileTargeting.TargetingData = userProfileAdapter.Output.TargetingData;
                }
                
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.UserProfile.UserProfileBAL.GetUserProfile(userCookie : {0})", userCookie));
            }
            return profileTargeting;
        }
    }
}
