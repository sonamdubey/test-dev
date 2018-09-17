using Bikewale.Entities.UserProfile;
using Bikewale.Interfaces.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Bikewale.Controllers.Shared
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 03 July 2018
    /// Description : Controller for Google Ads (Targeting)
    /// </summary>
    public class GoogleAdsController : Controller
    {
        private static string userCookieName = "BWC";
        private readonly IUserProfileBAL _userProfileBAL;
        public GoogleAdsController(IUserProfileBAL userProfileBAL)
        {
            _userProfileBAL = userProfileBAL;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 03 July 2018
        /// Description : Action Method to get User Profile targeting strings
        /// </summary>
        public ActionResult GetUserProfileTargeting(string cookieId="")
        {
            UserProfileTargeting targetingInfo = null;
            String userCookie = "";

            if (String.IsNullOrEmpty(cookieId))
            {
                HttpCookie cookie = Request.Cookies[userCookieName];
                if (cookie != null)
                {
                    userCookie = cookie.Value;
                }
            }
            else
            {
                userCookie = cookieId;
            }
                
            //targetingInfo = _userProfileBAL.GetUserProfile(userCookie);

            return View("~/UI/Views/UserProfile/_UserProfileTargeting.cshtml", targetingInfo);
        }
    }
}
