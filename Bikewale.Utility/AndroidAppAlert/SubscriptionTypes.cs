
namespace Bikewale.Utility.AndroidAppAlert
{
    public class SubscriptionTypes
    {
        public const string _Global = "/topics/testbwnotification";
        public const string _NewsArticles = "/topics/testbwnewsnotification";
        public const string _NewBikes = "/topics/testbwbikelaunchnotification";
        public const string _UpcomingBikes = "/topics/testbwupcomingbikenotification";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public static string GetSubscriptionType(ushort subId)
        {
            string subType = string.Empty;
            switch (subId)
            {
                case 1:
                    subType = _Global;
                    break;
                case 2:
                    subType = _NewsArticles;
                    break;
                case 3:
                    subType = _NewBikes;
                    break;
                case 4:
                    subType = _UpcomingBikes;
                    break;
                default:
                    subType = _NewsArticles;
                    break;
            }

            return subType;
        }

    }


}
