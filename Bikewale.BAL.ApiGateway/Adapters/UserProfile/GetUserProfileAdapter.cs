using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using SpamFilter.Service.ProtoClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Bikewale.Entities.UserProfile;
using Bikewale.Utility;
using Bikewale.Notifications;
using Bhrigu;
using BhriguServiceCaller;

namespace Bikewale.BAL.ApiGateway.Adapters.UserProfile
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : Adapter to call the microservice for getting the User Profile data.
    /// </summary>
    public class GetUserProfileAdapter : AbstractApiGatewayAdapter<string, UserProfileTargeting, UserProfileResponse>
    {
        public GetUserProfileAdapter() : base(BWConfiguration.Instance.UserProfileServiceModuleName, "GetUserProfile")
        {
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 04 July 2018
        /// Description : Overriden function to Build the Request for calling Bhrigu Microservice
        /// </summary>
        protected override IMessage BuildRequest(string userCookie)
        {
            UserProfileRequestBuilder profileRequestBuilder = null;
            try
            {
                if (!string.IsNullOrEmpty(userCookie))
                {
                    profileRequestBuilder = new UserProfileRequestBuilder(Application.Bikewale, userCookie);

                    #region order of adding the requests should be the same as the keys present in `UserProfileKeysEnum`
                    profileRequestBuilder.AddValuePreferenceKeyQuery(BWUserProfilePreferenceKeys.BIKEPREFERENCE);
                    profileRequestBuilder.AddTopKKeysQuery(BWUserProfileKeys.MODELS, 5);
                    profileRequestBuilder.AddTopKKeysQuery(BWUserProfileKeys.BODYSTYLE, 5);
                    profileRequestBuilder.AddNumberSubKeysQuery(BWUserProfileKeys.MODELSID);
                    #endregion

                    return profileRequestBuilder.GetUserProfileRequest();
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.UserProfile.GetUserProfileAdapter.BuildRequest()");
            }
            return null;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 04 July 2018
        /// Description : Overriden function to Build the Response provided by the Bhrigu Microservice
        /// </summary>
        protected override UserProfileTargeting BuildResponse(IMessage responseMessage)
        {
            UserProfileTargeting userProfileTargetingInfo = null;
            try
            {
                UserProfileResponse userProfile = responseMessage as UserProfileResponse;
                if (userProfile != null && userProfile.Response != null && userProfile.Response.Any())
                {
                    ICollection<Result> userProfileResponse = userProfile.Response;
                    userProfileTargetingInfo = new UserProfileTargeting();
                    ICollection<KeyValuePair<UserProfileKeysEnum, string>> targetingData = new List<KeyValuePair<UserProfileKeysEnum, string>>();
                    foreach (Result profile in userProfileResponse)
                    {
                        targetingData.Add(new System.Collections.Generic.KeyValuePair<UserProfileKeysEnum, string>((UserProfileKeysEnum)profile.Sequence, profile.Result_));
                    }
                    userProfileTargetingInfo.TargetingData = targetingData;
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.UserProfile.GetUserProfileAdapter.BuildResponse()");
            }
            return userProfileTargetingInfo;
        }
    }
}
