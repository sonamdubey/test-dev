using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Utility;
using System.Collections.Generic;

namespace Bikewale.Service.Utilities
{
    /// <summary>
    /// Created by  :     Sangram Nandkhile on 03 Mar 2016
    /// Summary     :     Class to hold create share url manipuation logic
    /// Modified By : Vivek Gupta on 13 May 2016.
    /// Description : public IEnumerable<CMSArticleSummary> GetShareUrl(IEnumerable<CMSArticleSummary> objCMSFArticles) Removed
    /// </summary>
    public class CMSShareUrl
    {
        /// <summary>
        /// Created by  :    Sangram Nandkhile on 03 Mar 2016
        /// Summary     :    This function is used to create shareUrl for all the CMS article
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public CMSContent GetShareUrl(CMSContent objCMSFArticles)
        {
            foreach (var article in objCMSFArticles.Articles)
            {
                article.ShareUrl = ReturnShareUrl(article.CategoryId, article.BasicId, article.ArticleUrl);
                article.FormattedDisplayDate = article.DisplayDate.ToString("dd MMMM yyyy");
            }
            return objCMSFArticles;
        }

        /// <summary>
        /// Created by  :    Sangram Nandkhile on 03 Mar 2016
        /// Summary     :    overload to fetch share URL for List<CMSArticleSummary>
        /// Modified on :    04 Mar 2016
        /// Summary     :    Removed switch case
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public List<CMSArticleSummary> GetShareUrl(List<CMSArticleSummary> objCMSFArticles)
        {
            foreach (var article in objCMSFArticles)
            {
                article.ShareUrl = ReturnShareUrl(article.CategoryId, article.BasicId, article.ArticleUrl);
                article.FormattedDisplayDate = article.DisplayDate.ToString("dd MMMM yyyy");
            }
            return objCMSFArticles;
        }

        /// <summary>
        /// Created by  :    Vivek Gupta 
        /// Summary     :    overload to fetch share URL for IEnumerable<CMSArticleSummary>
        /// Modified on :    11-5-2016
        /// Summary     :    Removed switch case
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public IEnumerable<CMSArticleSummary> GetShareUrl(IEnumerable<CMSArticleSummary> objCMSFArticles)
        {
            foreach (var article in objCMSFArticles)
            {
                article.ShareUrl = ReturnShareUrl(article.CategoryId, article.BasicId, article.ArticleUrl);
                article.FormattedDisplayDate = article.DisplayDate.ToString("dd MMMM yyyy");
            }
            return objCMSFArticles;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 03 Mar 2016
        /// Summary     :   Common function to return shareurl as per categoryid of the article
        /// Modified on :   04 Mar 2016
        /// Summary     :   Switch case has been added to cater common function calls
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="basicId"></param>
        /// <param name="articleUrl"></param>
        /// <returns></returns>
        public string ReturnShareUrl(ushort categoryId, ulong basicId, string articleUrl)
        {
            EnumCMSContentType contentType = (EnumCMSContentType)categoryId;
            switch (contentType)
            {
                case EnumCMSContentType.News:
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.AutoExpo2018:
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