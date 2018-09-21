using Bikewale.DTO.UserProfile;
using Bikewale.Entities.UserProfile;
using Bikewale.Interfaces.UserProfile;
using Bikewale.Service.AutoMappers.UserProfile;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.UserProfile
{
    public class GoogleAdsController : CompressionApiController
    {

        private readonly IUserProfileBAL _userProfileBAL;

        public GoogleAdsController(IUserProfileBAL userProfileBAL)
        {
            _userProfileBAL = userProfileBAL;
        }

        /// <summary>
        /// Created By : Deepak Israni on 6 July 2018
        /// Description : API to get user profile data from Bhrigu and return it client side for targeting.
        /// </summary>
        /// <param name="cookieId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/userprofiletargeting/")]
        public IHttpActionResult GetUserProfileTargeting(string cookieId = "")
        {
            UserProfileTargeting targetingInfo = null;
            UserProfileTargetingDTO targetingOutput = null;
            string cookieValue = cookieId, bwcValue = "";

            try
            {
                if (String.IsNullOrEmpty(cookieId))
                {
                    CookieHeaderValue cookieData = null;
                    ICollection<CookieHeaderValue> cookieList = Request.Headers.GetCookies();
                    if (cookieList != null)
                    {
                        cookieData = cookieList.FirstOrDefault();
                    }
                    if (cookieData != null)
                    {
                        bwcValue = cookieData["BWC"].Value;
                        cookieValue = bwcValue;
                    }
                }

                targetingInfo = _userProfileBAL.GetUserProfile(cookieValue);

                if (targetingInfo != null)
                {
                    targetingOutput = UserProfileMapper.Convert(targetingInfo);
                }
                else
                {
                    targetingOutput = new UserProfileTargetingDTO
                    {
                        TargetingData = new Dictionary<string, string>()
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.UserProfile.GoogleAdsController.GetUserProfileTargeting");
                return BadRequest();
            }
            return Ok(targetingOutput);
        }
    }
}