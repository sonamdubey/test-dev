using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Bikewale.Mobile.TrackDay
{
    public class Details : System.Web.UI.Page
    {
        protected bool androidApp;
        protected uint pageViews = 0;

        protected String _trackdayId = String.Empty;
        protected ArticleDetails objTrackDay = null;
        protected CMSContent objTrackDayArticles = null;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Form.Action = Request.RawUrl;

            if (!String.IsNullOrEmpty(Request.QueryString["isapp"]))
                bool.TryParse(Request.QueryString["isapp"], out androidApp);


            if (!IsPostBack)
            {
                ProcessQS();

                int _basicId = 0;

                if (Int32.TryParse(_trackdayId, out _basicId))
                {
                    GetTrackDayDetails(_basicId);
                    GetRelatedArticles();

                }
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", true);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void GetRelatedArticles()
        {
            throw new NotImplementedException();
        }

        private void GetTrackDayDetails(int _basicId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Bikewale.BAL.EditCMS.Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objTrackDay = _cache.GetNewsDetails(Convert.ToUInt32(_basicId));
                    objTrackDayArticles = _cache.GetArticlesByCategoryList(((int)EnumCMSContentType.TrackDay).ToString(), 1, 10, 0, 0);

                    if (objTrackDayArticles != null && objTrackDayArticles.Articles != null)
                        objTrackDayArticles.RecordCount = (uint)objTrackDayArticles.Articles.Count;

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (objTrackDayArticles != null && objTrackDayArticles.RecordCount < 1)
                {
                    Response.Redirect("/m/news/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        ///// <summary>
        ///// PopulateWhere to set news details
        ///// </summary>
        //private void GetNewsData()
        //{
        //    newsTitle = objNews.Title;
        //    author = objNews.AuthorName;
        //    displayDate = objNews.DisplayDate.ToString();
        //    newsContent = objNews.Content;
        //    nextPageTitle = objNews.NextArticle.Title;
        //    prevPageTitle = objNews.PrevArticle.Title;
        //    pageViews = objNews.Views;
        //    pageUrl = _newsId + '-' + objNews.ArticleUrl + ".html";
        //    if (objNews != null && objNews.VehiclTagsList.Count > 0)
        //    {
        //        _taggedMakeObj = objNews.VehiclTagsList.FirstOrDefault().MakeBase;
        //        FetchMakeDetails();
        //    }
        //    if (!String.IsNullOrEmpty(objNews.NextArticle.ArticleUrl))
        //        nextPageUrl = objNews.NextArticle.BasicId + "-" + objNews.NextArticle.ArticleUrl + ".html";

        //    if (!String.IsNullOrEmpty(objNews.PrevArticle.ArticleUrl))
        //        prevPageUrl = objNews.PrevArticle.BasicId + "-" + objNews.PrevArticle.ArticleUrl + ".html";
        //}

        /// <summary>
        /// Populate Where to process query string and get carwale new basicid against bikewale basicid
        /// </summary>
        private void ProcessQS()
        {
            if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                //Check if basic id exists in mapped carwale basic id log **/
                string basicid = BasicIdMapping.GetCWBasicId(Request["id"]);
                //if id exists then redirect url to new basic id url
                if (!basicid.Equals(Request.QueryString["id"]))
                {
                    string newUrl = string.Format("/m/trackday2016/{0}-{1}.html", basicid, Request["t"]);
                    CommonOpn.RedirectPermanent(newUrl);
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["id"]))
                        _trackdayId = Request.QueryString["id"];
                }
            }
            else
            {
                Response.Redirect("/m/news/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
    }
}