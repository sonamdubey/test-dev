using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace Bikewale.News
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 16 Nov 2012
    ///     Summary : Class for creating the news feed.
    /// </summary>
    public class NewsFeed : System.Web.UI.Page
    {
        //protected Repeater rptNews;
        //private string selectClause = "", fromClause = "", whereClause = "", orderByClause = "";
        private int CurrentPageIndex = 1;//, PageSize = 10;
        //private string _slug = string.Empty;
        //private string _subCat = string.Empty;
        //private string relatedItem1 = string.Empty, relatedItem2 = string.Empty, relatedItem3 = string.Empty, relatedItem4 = string.Empty;
        //private string cwConnectionString = string.Empty;
        //XmlTextWriter writer;
        //DataSet ds = new DataSet();
        //DataSet dsRelated = new DataSet();
        //CMSContent objNews = null;
        //CMSContent objRelatedNews = null;
        //IEnumerable<ArticleSummary> articles = null;
        //IEnumerable<ArticleSummary> relatedArticles = null;
        protected string _title = "";
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
        void Page_Load(object Sender, EventArgs e)
        {
            //writer = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);
            if (Request["pn"] != null && Request.QueryString["pn"].ToString().Trim() != string.Empty)
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    CurrentPageIndex = Convert.ToInt32(Request.QueryString["pn"]);
            }
            BindNews();
        }

        private void BindNews()
        {
            //SqlCommand cmd = new SqlCommand();
            //bool hasRows = false;

            try
            {
                //using (IUnityContainer container = new UnityContainer())
                //{
                //    container.RegisterType<IArticles, Articles>()
                //               .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                //               .RegisterType<ICacheManager, MemcacheManager>();
                //    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                //    List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                //    categorList.Add(EnumCMSContentType.AutoExpo2016);
                //    categorList.Add(EnumCMSContentType.News);
                //    string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                //    categorList.Clear();
                //    categorList = null;

                //    objNews = _cache.GetArticlesByCategoryList(contentTypeList, 1, 50, 0, 0);
                //}

                try
                {
                    //sets the base URI for HTTP requests
                    string _apiUrl = string.Format("{0}/news/feed/?application=2", Utility.BWConfiguration.Instance.CwApiHostUrl);

                    if (CurrentPageIndex > 1)
                    {
                        _apiUrl = string.Format("{0}/news/page/{1}/feed/?application=2", Utility.BWConfiguration.Instance.CwApiHostUrl, CurrentPageIndex);
                    }

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(_apiUrl) as System.Net.HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(response.GetResponseStream());

                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.ContentType = "text/xml";
                    Response.Cache.SetCacheability(HttpCacheability.Public);

                    Response.Write(xmlDoc.InnerXml);
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                //if (objNews.RecordCount > 0)
                //{
                //    hasRows = true;
                //    articles = objNews.Articles;
                //    WriteRSSPrologue();
                //    GetRelatedItems();
                //    int articleCount = articles.Count();
                //    for (int i = 0; i < articleCount; i++)
                //    {
                //        CreateItem(i);
                //    }
                //    WriteRSSClosing();

                //    writer.Flush();
                //    writer.Close();

                //    Response.ContentEncoding = System.Text.Encoding.UTF8;
                //    Response.ContentType = "text/xml";
                //    Response.Cache.SetCacheability(HttpCacheability.Public);
                //}
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            //if (!hasRows)
            //{
            //    Response.Redirect("/news/", false);
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //    this.Page.Visible = false;
            //}
            finally
            {
                Response.End();
            }            
        }
    }   // End of class
}   // End of namespace