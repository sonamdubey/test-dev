using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bikewale.Service.Utilities
{
    /// <summary>
    /// a class to hold create share url manipuation logic
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
            string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            foreach (var article in objCMSFArticles.Articles)
            {
                EnumCMSContentType contentType = (EnumCMSContentType)article.CategoryId;
                switch (contentType)
                {
                    case EnumCMSContentType.News:
                    case EnumCMSContentType.AutoExpo2016:
                        article.ShareUrl = _bwHostUrl + "/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                        break;
                    case EnumCMSContentType.Features:
                        article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                        break;
                    case EnumCMSContentType.RoadTest:
                        article.ShareUrl = _bwHostUrl + "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                        break;
                    case EnumCMSContentType.SpecialFeature:
                        article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                        break;
                    default:
                        break;
                }
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
            string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            foreach (var article in objCMSFArticles)
            {
                EnumCMSContentType contentType = (EnumCMSContentType)article.CategoryId;
                switch (contentType)
                {
                    case EnumCMSContentType.News:
                    case EnumCMSContentType.AutoExpo2016:
                        article.ShareUrl = _bwHostUrl + "/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                        break;
                    case EnumCMSContentType.Features:
                        article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                        break;
                    case EnumCMSContentType.RoadTest:
                        article.ShareUrl = _bwHostUrl + "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                        break;
                    case EnumCMSContentType.SpecialFeature:
                        article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                        break;
                    default:
                        break;
                }
                article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
            }
            return objCMSFArticles;
        }
    }
}