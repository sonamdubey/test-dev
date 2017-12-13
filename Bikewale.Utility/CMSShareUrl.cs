
using Bikewale.Entities.CMS;

namespace Bikewale.Utility
{
    public static class CMSShareUrl
    {

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 01 Dec 2017
        /// Summary     :   Common function to return shareurl as per categoryid of the article
        /// Modified on :   04 Mar 2016
        /// Summary     :   Switch case has been added to cater common function calls
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="basicId"></param>
        /// <param name="articleUrl"></param>
        /// <returns></returns>
        public static string ReturnShareUrl(ushort categoryId, ulong basicId, string articleUrl)
        {
            EnumCMSContentType contentType = (EnumCMSContentType)categoryId;
            switch (contentType)
            {
                case EnumCMSContentType.News:
                case EnumCMSContentType.AutoExpo2016:
                    return string.Format("{0}/news/{1}-{2}.html", BWConfiguration.Instance.BwHostUrlForJs, basicId, articleUrl);
                case EnumCMSContentType.Features:
                    return string.Format("{0}/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrlForJs, articleUrl, basicId);
                case EnumCMSContentType.RoadTest:
                    return string.Format("{0}/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrlForJs, articleUrl, basicId);
                case EnumCMSContentType.SpecialFeature:
                    return string.Format("{0}/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrlForJs, articleUrl, basicId);
            }
            return string.Empty;
        }
    }
}
