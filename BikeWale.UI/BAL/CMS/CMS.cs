using Bikewale.Automappers;
using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using log4net;
using log4net.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Bikewale.BAL.CMS
{
    /// <summary>
    /// Created By : Ashish G.  Kamble on 4 June 2017
    /// Class have the functions related to editcms BAL
    /// </summary>
    public class CMS : ICMS
    {
        private readonly IArticles _objArticles;
        private readonly ICacheManager _cache = null;
        /// <summary>
        /// constructor to initialize the dependencies
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objArticles"></param>
        public CMS(ICacheManager cache, IArticles objArticles)
        {
            _cache = cache;
            _objArticles = objArticles;
        }


        #region GetArticleDetailsPage method
        /// <summary>
        /// Written By : Ashish G. Kamble on 2 June 2017
        /// Summary : Function to get the data for bikewale new details api. All data comes from cache.
        /// </summary>
        /// <param name="basicId">Mandatory field</param>
        /// <returns>Returns JSON for new details page</returns>
        public string GetArticleDetailsPage(uint basicId)
        {
            string articleDetails = string.Empty;

            if (basicId > 0)
            {
                articleDetails = _cache.GetFromCache<String>(string.Format("BW_ArticleDetails_Mobile_{0}", basicId), new TimeSpan(1, 0, 0), () => GetArticleDetailsPageJSON(basicId));
                if (!String.IsNullOrEmpty(articleDetails))
                {
                    HostingEnvironment.QueueBackgroundWorkItem(f => _objArticles.UpdateViewCount(basicId));
                }
            }
            return articleDetails;
        }   // end of GetArticleDetailsPage 
        #endregion


        #region GetArticleDetailsPageJSON method
        /// <summary>
        /// Written By : Ashish G. Kamble on 2 June 2017
        /// Summary : Function to get the editorial content for details of given basic id. Function also cache the data for given basic id.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Returns data in json format</returns>
        private string GetArticleDetailsPageJSON(uint basicId)
        {
            string articleDetails = string.Empty;
            DTO.CMS.Articles.HtmlContent htmlContent = null;
            CMSArticleDetails objCMSFArticles = null;

            try
            {
                if (basicId > 0)
                {
                    ArticleDetails objNews = _objArticles.GetNewsDetails(basicId);

                    if (objNews != null)
                    {
                        objCMSFArticles = new CMSArticleDetails();

                        // Convert Entity to DTO
                        objCMSFArticles = CMSMapper.Convert(objNews);

                        if (objNews.TagsList != null && objNews.TagsList.Count > 0)
                        {
                            objNews.TagsList.Clear();
                            objNews.TagsList = null;
                        }

                        if (objNews.VehiclTagsList != null && objNews.VehiclTagsList.Count > 0)
                        {
                            objNews.VehiclTagsList.Clear();
                            objNews.VehiclTagsList = null;
                        }

                        objCMSFArticles.FormattedDisplayDate = objNews.DisplayDate.ToString("dd MMM yyyy, hh:mm tt");

                        // Remove the html tags in the article content for mobile platforms
                        Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(objNews.Content);

                        if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                        {
                            htmlContent = new DTO.CMS.Articles.HtmlContent();
                            htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                            if (objContent.HtmlItems != null)
                            {
                                objContent.HtmlItems.Clear();
                                objContent.HtmlItems = null;
                            }

                            objCMSFArticles.htmlContent = htmlContent;
                        }

                        objCMSFArticles.ShareUrl = new CMSShareUrl().ReturnShareUrl(objCMSFArticles);

                        // serialize the object to string
                        articleDetails = Newtonsoft.Json.JsonConvert.SerializeObject(objCMSFArticles);                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.CMS.CMS.GetArticleDetailsPageJSON");
               
            }
            return articleDetails;
        }   // end of GetArticleDetailsPageJSON 
        #endregion


        #region GetArticleDetailsPages method
        /// <summary>
        /// Written By : Ashish G. Kamble on 2 June 2017
        /// Summary : Function to get the editorial content for details of given basic id. Function also cache the data for given basic id.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public string GetArticleDetailsPages(uint basicId)
        {
            string articleDetails = string.Empty;

            if (basicId > 0)
            {
                articleDetails = _cache.GetFromCache<String>(string.Format("BW_ArticleDetails_Mobile_{0}", basicId), new TimeSpan(1, 0, 0), () => GetArticleDetailsPagesJSON(basicId));
                if (!String.IsNullOrEmpty(articleDetails))
                {
                    HostingEnvironment.QueueBackgroundWorkItem( f => _objArticles.UpdateViewCount(basicId));
                }
            }
            return articleDetails;
        }  // end of GetArticleDetailsPages
        #endregion


        #region GetArticleDetailsPagesJSON method
        /// <summary>
        /// Written By : Ashish G. Kamble on 4 June 2017
        /// Summary : Function to get data for edit cms articles where multiple pages exist for the article. (e.g. Expert Reviews, Features, Tipes and advices).
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Returns data in json format</returns>
        private string GetArticleDetailsPagesJSON(uint basicId)
        {
            string articleDetails = string.Empty;
            ArticlePageDetails objFeaturedArticles = null;

            try
            {
                if (basicId > 0)
                {
                    objFeaturedArticles = _objArticles.GetArticleDetails(basicId);

                    if (objFeaturedArticles != null)
                    {
                        CMSArticlePageDetails objCMSFArticles = new CMSArticlePageDetails();

                        // map entity to DTO
                        objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                        if (objFeaturedArticles != null)
                        {
                            if (objFeaturedArticles.PageList != null && objFeaturedArticles.PageList.Count > 0)
                            {
                                objFeaturedArticles.PageList.Clear();
                                objFeaturedArticles.PageList = null;
                            }

                            if (objFeaturedArticles.TagsList != null && objFeaturedArticles.TagsList.Count > 0)
                            {
                                objFeaturedArticles.TagsList.Clear();
                                objFeaturedArticles.TagsList = null;
                            }

                            if (objFeaturedArticles.VehiclTagsList != null && objFeaturedArticles.VehiclTagsList.Count > 0)
                            {
                                objFeaturedArticles.VehiclTagsList.Clear();
                                objFeaturedArticles.VehiclTagsList = null;
                            }
                        }

                        objCMSFArticles.FormattedDisplayDate = objFeaturedArticles.DisplayDate.ToString("dd MMMM yyyy, hh:mm tt");

                        // Remove all the HTML content from the article and append new content
                        foreach (var page in objCMSFArticles.PageList)
                        {
                            Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(page.Content);

                            if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                            {
                                DTO.CMS.Articles.HtmlContent htmlContent = new DTO.CMS.Articles.HtmlContent();
                                htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                                page.htmlContent = htmlContent;
                                page.Content = "";

                                objContent.HtmlItems.Clear();
                                objContent.HtmlItems = null;
                            }
                        }

                        objCMSFArticles.ShareUrl = new CMSShareUrl().ReturnShareUrl(objCMSFArticles);

                        // serialize the object to string
                        articleDetails = Newtonsoft.Json.JsonConvert.SerializeObject(objCMSFArticles);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.CMS.CMS.GetArticleDetailsPagesJSON");
               
            }

            return articleDetails;
        } // end of GetArticleDetailsPagesJSON 
        #endregion

    }   // class
}   // namespace
