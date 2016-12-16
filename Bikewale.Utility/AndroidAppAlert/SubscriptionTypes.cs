
namespace Bikewale.Utility.AndroidAppAlert
{
    public class SubscriptionTypes
    {
        public const string _Global = "/topics/{0}bwnotification";
        public const string _NewsArticles = "/topics/{0}bwnewsnotification";
        public const string _NewBikes = "/topics/{0}bwbikelaunchnotification";
        public const string _UpcomingBikes = "/topics/{0}bwupcomingbikenotification";
        public const string _TrackDay = "/topics/{0}bwtrackdaynotification";

        /// <summary>
        /// Created By : Sushil Kumar on 15th Dec 2016
        /// Deascription : Android App notification categories
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public static string GetSubscriptionType(ushort subId)
        {
            string subType = string.Empty;
            switch (subId)
            {
                case 0:
                    subType = string.Format(_Global, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 1:  //non featured news
                case 2:  //featured news
                    subType = string.Format(_NewsArticles, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 3:
                    subType = string.Format(_NewBikes, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 4:
                    subType = string.Format(_UpcomingBikes, BWConfiguration.Instance.FCMEnvironment);
                    break;
                case 5:
                    subType = string.Format(_TrackDay, BWConfiguration.Instance.FCMEnvironment);
                    break;
                default:
                    subType = string.Format(_NewsArticles, BWConfiguration.Instance.FCMEnvironment);
                    break;
            }

            return subType;
        }

    }


}
