using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.BAL.CMS;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Entities.CMS.Articles;
using System.Configuration;

namespace Bikewale.Mobile
{
    public class MobileDefault_Old : System.Web.UI.Page
    {
        protected Repeater rptFeaturedArticles;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindFeaturedArticles();
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 May 2014
        /// Summary : Function to get and show the featured articles data .
        /// Modified By : get featured alrticles from api asynchronously
        /// </summary>
        protected async void BindFeaturedArticles()
        {
            try
            {
                List<EnumCMSContentType> _contentList = new List<EnumCMSContentType>();
                _contentList.Add(EnumCMSContentType.News);
                _contentList.Add(EnumCMSContentType.RoadTest);
                _contentList.Add(EnumCMSContentType.Features);
                _contentList.Add(EnumCMSContentType.ComparisonTests);

                string _contentTypeList = CommonOpn.GetContentTypesString(_contentList);
                Trace.Warn("content ty " + _contentTypeList);
                List<ArticleSummary> _objFeaturedList = null;

                string _apiUrl = "webapi/article/featuredlist/?applicationid=2&contenttypes=" + _contentTypeList + "&totalrecords=" + 4;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objFeaturedList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objFeaturedList);
                }
                
                if (_objFeaturedList != null && _objFeaturedList.Count > 0)
                {
                    rptFeaturedArticles.DataSource = _objFeaturedList;
                    rptFeaturedArticles.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Function to get the content articles link to the content details page.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        protected string GetFeaturedArticlesLink(string url, int contentId, ushort categoryId)
        {
            string articleUrl = string.Empty;

            switch (categoryId)
            {
                case (ushort) EnumCMSContentType.RoadTest :
                    articleUrl = "/m/road-tests/" + url + "-" + contentId + ".html";
                    break;

                case (ushort)EnumCMSContentType.News:
                    articleUrl = "/m/news/" + contentId + "-" + url + ".html";
                    break;

                case (ushort)EnumCMSContentType.Features:
                    articleUrl = "/m/features/" + url + "-" + contentId + "/";
                    break;

                case (ushort)EnumCMSContentType.ComparisonTests:
                    articleUrl = "/m/comparos/" + url + "-" + contentId + ".html";
                    break;

                default:
                    articleUrl = "#";
                    break;
            }

            return articleUrl;
        }   // End of GetFeaturedArticlesLink

    }   // class
}   // namespace