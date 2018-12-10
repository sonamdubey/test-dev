using System;
using Carwale.Utility;
using Carwale.DAL.AdTargeting;

namespace Adtargeting
{
    public static partial class UserData
    {
        public partial class UserDataClient
        {
            private readonly static int _deadline = CWConfiguration.AdtargetingChannelTime;            
            public UserDataClient(bool wrapper) : base(AdTargetingChannel.Channel)
            {
            }

            private static DateTime GetForwardTime(int incrementMillisecond)
            {
                return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
            }

            public UserDataResponse GetUserData(CookieRequest cwCookie, string threadProperty)
            {
                return GetUserData(cwCookie, null, GetForwardTime(_deadline));
            }

            public ElectricCarViewStatus IsElectricCarViewed(CookieRequest request)
            {
                return IsElectricCarViewed(request, null, GetForwardTime(_deadline));
            }
        }
    }
}
