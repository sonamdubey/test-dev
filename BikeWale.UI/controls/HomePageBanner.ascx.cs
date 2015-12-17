using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.CMS;
using Bikewale.Entities.CMS;
using Bikewale.BAL.CMS;
using System.Net.Http;
using Bikewale.Entities.CMS.Articles;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http.Headers;

namespace Bikewale.Controls
{
    public class HomePageBanner : System.Web.UI.UserControl
    {
        protected Repeater rptFullImage, rptThumbBanner;
        private string _category = "0";

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private string _topRecords = "4";

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);            
        }
        
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindFeaturedArticles();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// PopulateWhere to get featured and recent articles from web api
        /// </summary>
        protected async void BindFeaturedArticles()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string _contentList = GetContentTypes();
                    List<ArticleSummary> objArticleList = null;
                   
                    switch (Convert.ToUInt16(Category))
                    {
                        case 0:
                            objArticleList = await GetFeaturedArticles(_contentList);
                            break;
                        default :
                             objArticleList = await GetMostRecentArticles(_contentList);
                            break;
                    }

                    rptFullImage.DataSource = objArticleList;
                    rptFullImage.DataBind();

                    rptThumbBanner.DataSource = objArticleList;
                    rptThumbBanner.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of BindFeaturedArticles

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get most recent article list from web api
        /// </summary>
        /// <param name="_contentList"></param>
        /// <returns></returns>
        private async Task<List<ArticleSummary>> GetMostRecentArticles(string _contentList)
        {
            List<ArticleSummary> _objArticleList = null;
            
            string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentList + "&totalrecords=" + TopRecords;

            using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                _objArticleList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
            }            

            return _objArticleList;
        }//End of GetMostRecentArticles

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get featured article list from web api
        /// </summary>
        /// <param name="_contentList"></param>
        /// <returns></returns>
        private async Task<List<ArticleSummary>> GetFeaturedArticles(string _contentList)
        {
            List<ArticleSummary> _objArticleList = null;

            string _apiUrl = "webapi/article/featuredlist/?applicationid=2&contenttypes=" + _contentList + "&totalrecords=" + TopRecords;

            using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                _objArticleList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
            }            

             return _objArticleList;

        }//End of GetFeaturedArticles

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get catagory list in string format
        /// </summary>
        /// <returns></returns>
        private string GetContentTypes()
        {
            List<EnumCMSContentType> contentList = new List<EnumCMSContentType>();
            string _contentTypes = string.Empty;
            ushort _contentType = 0;

            switch (Convert.ToUInt16(Category))
            {
                case 0:
                    contentList.Add(EnumCMSContentType.News);
                    contentList.Add(EnumCMSContentType.RoadTest);
                    contentList.Add(EnumCMSContentType.Features);
                    contentList.Add(EnumCMSContentType.ComparisonTests);
                    break;
                case (int)EnumCMSContentType.News:
                    contentList.Add(EnumCMSContentType.News);
                    break;
                case (int)EnumCMSContentType.RoadTest:
                    contentList.Add(EnumCMSContentType.RoadTest);
                    break;
            }

            foreach (var item in contentList)
            {
                _contentType = (ushort)item;
                _contentTypes += _contentType.ToString() + ',';
            }

            _contentTypes = _contentTypes.Remove(_contentTypes.LastIndexOf(','));

            return _contentTypes;

        } //End of GetContentTypes

        protected string GetFeaturedArticlesLink(string url, int contentId, ushort categoryId)
        {
            string articleUrl = string.Empty;

            switch (categoryId)
            {
                case (ushort)EnumCMSContentType.RoadTest:
                    articleUrl = "/road-tests/" + url + "-" + contentId + ".html";
                    break;

                case (ushort)EnumCMSContentType.News:
                    articleUrl = "/news/" + contentId + "-" + url + ".html";
                    break;

                case (ushort)EnumCMSContentType.Features:
                    articleUrl = "/features/" + url + "-" + contentId + "/";
                    break;

                case (ushort)EnumCMSContentType.ComparisonTests:
                    articleUrl = "/comparos/" + url + "-" + contentId + ".html";
                    break;

                default:
                    articleUrl = "#";
                    break;
            }

            return articleUrl;
        }   // End of GetFeaturedArticlesLink
    }
}