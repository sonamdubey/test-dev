
namespace Bikewale.Utility.AndroidAppAlert
{
    public class SubscriptionTypes
    {
        public const string _Global = "/topics/{0}bwnotification";
        public const string _NewsArticles = "/topics/{0}bwnewsnotification";
        public const string _NewBikes = "/topics/{0}bwbikelaunchnotification";
        public const string _UpcomingBikes = "/topics/{0}bwupcomingbikenotification";

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
                    subType = string.Format(_Global, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 2:
                    subType = string.Format(_NewsArticles, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 3:
                    subType = string.Format(_NewBikes, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 4:
                    subType = string.Format(_UpcomingBikes, BWConfiguration.Instance.FCMEnvironment);
                    break;
                default:
                    subType = string.Format(_NewsArticles, BWConfiguration.Instance.FCMEnvironment);
                    break;
            }

            return subType;
        }

    }


}
