using Carwale.BL.UserProfiling;
using Carwale.DTOs.ES;
using Carwale.Entity.Enum;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.UserProfiling
{
    public class UserProfileController : Controller
    {
        public ActionResult GetAdTagetingData(string cwcCookieId, Platform platform)
        {
            UserProfilingDto userProfilingData = new UserProfilingDto()
            {
                AdTargetingData = UserProfilingBL.GetGrpcUserProfile(cwcCookieId, platform)
            };
            return PartialView("~/Views/UserProfile/_AdTargetingScriptView.cshtml", userProfilingData);
        }
	}
}