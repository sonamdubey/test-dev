using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Service.Utilities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 04 Mar 2016
    ///           : a class to hold create share url manipuation logic
    /// </summary>
    public class CMSShareUrl
    {
        /// <summary>
        /// This function is used to create shareUrl for all the CMS article
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public CMSContent GetShareUrl(CMSContent objCMSFArticles)
        {
            foreach (var article in objCMSFArticles.Articles)
            {
                article.ShareUrl = ReturnShareUrl(article);
                article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
            }
            return objCMSFArticles;
        }

        /// <summary>
        /// overload to fetch share URL for List<CMSArticleSummary>
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public List<CMSArticleSummary> GetShareUrl(List<CMSArticleSummary> objCMSFArticles)
        {
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            foreach (var article in objCMSFArticles)
            {
                article.ShareUrl = ReturnShareUrl(article);
                article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
            }
            return objCMSFArticles;
        }

        /// <summary>
        /// overload to fetch share URL for List<CMSArticlePageDetails>
        /// </summary>
        /// <param name="objCMSFArticles"></param>
        /// <returns></returns>
        public string GetShareUrl(CMSArticlePageDetails article)
        {
            return ReturnShareUrl(article);
        }
        /// <summary>
        /// Common function to return shareurl as per categoryid of the article
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
                    articleSummary.ShareUrl = string.Format("{0}/news/{1}-{2}.html",_bwHostUrl,articleSummary.BasicId,articleSummary.ArticleUrl); //_bwHostUrl + "/news/" + articleSummary.BasicId + "-" + articleSummary.ArticleUrl + ".html";
                    break;
                case EnumCMSContentType.Features:
                    articleSummary.ShareUrl = string.Format("{0}/features/{1}-{2}/",_bwHostUrl,articleSummary.ArticleUrl,articleSummary.BasicId); // _bwHostUrl + "/features/" + articleSummary.ArticleUrl + "-" + articleSummary.BasicId; ;
                    break;
                case EnumCMSContentType.RoadTest:
                    articleSummary.ShareUrl = string.Format("{0}/road-tests/{1}-{2}.html",_bwHostUrl,articleSummary.ArticleUrl,articleSummary.BasicId); //_bwHostUrl + "/road-tests/" + articleSummary.ArticleUrl + "-" + articleSummary.BasicId + ".html";
                    break;
                case EnumCMSContentType.SpecialFeature:
                    articleSummary.ShareUrl = string.Format("{0}/features/{1}-{2}/",_bwHostUrl,articleSummary.ArticleUrl,articleSummary.BasicId);  //_bwHostUrl + "/features/" + articleSummary.ArticleUrl + "-" + articleSummary.BasicId+"/"; ;
                    break;
                default:
                    break;
            }
            return articleSummary.ShareUrl;
        }
    }
}