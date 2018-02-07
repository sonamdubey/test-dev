using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Utility;
using System.Collections.Generic;

namespace Bikewale.BAL.CMS
{
    /// <summary>
    /// Created by  : Ashish G. Kamble on 2 June 2017
    /// Summary     : Class to hold create share url manipuation logic. Class is moved from service layer to BAL by Ashish
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
                article.ShareUrl = ReturnShareUrl(article);
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
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            foreach (var article in objCMSFArticles)
            {
                article.ShareUrl = ReturnShareUrl(article);
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
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            foreach (var article in objCMSFArticles)
            {
                article.ShareUrl = ReturnShareUrl(article);
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
        /// <param name="articleSummary"></param>
        /// <returns></returns>
        public string ReturnShareUrl(CMSArticleSummary articleSummary)
        {
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            EnumCMSContentType contentType = (EnumCMSContentType)articleSummary.CategoryId;
            switch (contentType)
            {
                case EnumCMSContentType.News:
                case EnumCMSContentType.AutoExpo2016:
                case EnumCMSContentType.AutoExpo2018:
                    articleSummary.ShareUrl = string.Format("{0}/news/{1}-{2}.html", _bwHostUrl, articleSummary.BasicId, articleSummary.ArticleUrl);
                    break;
                case EnumCMSContentType.Features:
                    articleSummary.ShareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                case EnumCMSContentType.RoadTest:
                    articleSummary.ShareUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                case EnumCMSContentType.SpecialFeature:
                    articleSummary.ShareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                default:
                    break;
            }
            return articleSummary.ShareUrl;
        }

    }
}
