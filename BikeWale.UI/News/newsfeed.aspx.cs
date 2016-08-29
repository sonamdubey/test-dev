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
        protected Repeater rptNews;
        //private string selectClause = "", fromClause = "", whereClause = "", orderByClause = "";
        private int CurrentPageIndex = 1;//, PageSize = 10;
        private string _slug = string.Empty;
        private string _subCat = string.Empty;
        private string relatedItem1 = string.Empty, relatedItem2 = string.Empty, relatedItem3 = string.Empty, relatedItem4 = string.Empty;
        private string cwConnectionString = string.Empty;
        XmlTextWriter writer;
        DataSet ds = new DataSet();
        DataSet dsRelated = new DataSet();
        CMSContent objNews = null;
        CMSContent objRelatedNews = null;
        IEnumerable<ArticleSummary> articles = null;
        IEnumerable<ArticleSummary> relatedArticles = null;
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
            writer = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);
            if (Request["pn"] != null && Request.QueryString["pn"].ToString().Trim() != string.Empty)
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    CurrentPageIndex = Convert.ToInt32(Request.QueryString["pn"]);
            }
            if (Request["slug"] != null && Request.QueryString["slug"].ToString().Trim() != string.Empty)
            {
                _slug = Request.QueryString["slug"].ToString();
            }
            if (Request["cat"] != null && Request.QueryString["cat"].ToString().Trim() != string.Empty)
            {
                _subCat = Request.QueryString["cat"].ToString();
            }
            BindNews();
        }

        private void BindNews()
        {
            //SqlCommand cmd = new SqlCommand();
            bool hasRows = false;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                               .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                               .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                    categorList.Add(EnumCMSContentType.AutoExpo2016);
                    categorList.Add(EnumCMSContentType.News);
                    string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                    categorList.Clear();
                    categorList = null;

                    objNews = _cache.GetArticlesByCategoryList(contentTypeList, 1, 50, 0, 0);
                }
                if (objNews.RecordCount > 0)
                {
                    hasRows = true;
                    articles = objNews.Articles;
                    WriteRSSPrologue();
                    GetRelatedItems();
                    int articleCount = articles.Count();
                    for (int i = 0; i < articleCount; i++)
                    {
                        CreateItem(i);
                    }
                    WriteRSSClosing();

                    writer.Flush();
                    writer.Close();

                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.ContentType = "text/xml";
                    Response.Cache.SetCacheability(HttpCacheability.Public);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (!hasRows)
            {
                Response.Redirect("/news/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            Response.End();
        }

        private void GetRelatedItems()
        {
            if (CurrentPageIndex == 1)
            {
                ArticleSummary firstArticle = articles.FirstOrDefault();
                ArticleSummary secondArticle = articles.Skip(1).FirstOrDefault();
                ArticleSummary thirdArticle = articles.Skip(2).FirstOrDefault();
                ArticleSummary forthArticle = articles.Skip(3).FirstOrDefault();
                relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", firstArticle.BasicId, firstArticle.ArticleUrl);
                relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", secondArticle.BasicId, secondArticle.ArticleUrl);
                relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", thirdArticle.BasicId, thirdArticle.ArticleUrl);
                relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", forthArticle.BasicId, forthArticle.ArticleUrl);
            }
            else
            {
                GetRelatedLinksFromDB();
                if (relatedArticles != null && relatedArticles.Count() > 0)
                {
                    ArticleSummary firstArticle = relatedArticles.FirstOrDefault();
                    ArticleSummary secondArticle = relatedArticles.Skip(1).FirstOrDefault();
                    ArticleSummary thirdArticle = relatedArticles.Skip(2).FirstOrDefault();
                    ArticleSummary forthArticle = relatedArticles.Skip(3).FirstOrDefault();
                    relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", firstArticle.BasicId, firstArticle.ArticleUrl);
                    relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", secondArticle.BasicId, secondArticle.ArticleUrl);
                    relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", thirdArticle.BasicId, thirdArticle.ArticleUrl);
                    relatedItem1 = String.Format("http://www.bikewale.com/news/{0}-{1}.html", forthArticle.BasicId, forthArticle.ArticleUrl);
                }
            }
        }

        private void GetRelatedLinksFromDB()
        {

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.AutoExpo2016);
                categorList.Add(EnumCMSContentType.News);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                objRelatedNews = _cache.GetArticlesByCategoryList(contentTypeList, 1, 50, 0, 0);
                relatedArticles = objRelatedNews != null && objRelatedNews.RecordCount > 0 ? objRelatedNews.Articles : null;
            }

        }

        /// <summary>
        /// Creates the items in the RSS documents in the Channel element.
        /// </summary>
        /// <param name="index">row number from which value is retrieved from the dataset</param>
        protected void CreateItem(int index)
        {
            if (index == 0)
            {
                ArticleSummary firstArticle = articles.FirstOrDefault();
                AddRSSItem(
                    firstArticle.BasicId.ToString(),
                    firstArticle.Title,
                    String.Format("http://www.bikewale.com/news/{0}-{1}.html", firstArticle.BasicId, firstArticle.ArticleUrl),
                    firstArticle.AuthorName,
                    firstArticle.DisplayDate.ToString(),
                    firstArticle.Views.ToString(),
                    Bikewale.Utility.Image.GetPathToShowImages(firstArticle.OriginalImgUrl, firstArticle.HostUrl, Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(firstArticle.OriginalImgUrl, firstArticle.HostUrl, Bikewale.Utility.ImageSize._642x361),
                    firstArticle.Description,
                    firstArticle.Description,
                    relatedItem2,
                    relatedItem3,
                    relatedItem4
                    );
            }
            else if (index == 1)
            {
                ArticleSummary secondArticle = articles.Skip(index).FirstOrDefault();
                AddRSSItem(
                    secondArticle.BasicId.ToString(),
                    secondArticle.Title,
                    String.Format("http://www.bikewale.com/news/{0}-{1}.html", secondArticle.BasicId, secondArticle.ArticleUrl),
                    secondArticle.AuthorName,
                    secondArticle.DisplayDate.ToString(),
                    secondArticle.Views.ToString(),
                    Bikewale.Utility.Image.GetPathToShowImages(secondArticle.OriginalImgUrl, secondArticle.HostUrl, Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(secondArticle.OriginalImgUrl, secondArticle.HostUrl, Bikewale.Utility.ImageSize._642x361),
                    secondArticle.Description,
                    secondArticle.Description,
                    relatedItem1,
                    relatedItem3,
                    relatedItem4
                    );
            }
            else if (index == 2)
            {
                ArticleSummary thirdArticle = articles.Skip(index).FirstOrDefault();
                AddRSSItem(
                    thirdArticle.BasicId.ToString(),
                    thirdArticle.Title,
                    String.Format("http://www.bikewale.com/news/{0}-{1}.html", thirdArticle.BasicId, thirdArticle.ArticleUrl),
                    thirdArticle.AuthorName,
                    thirdArticle.DisplayDate.ToString(),
                    thirdArticle.Views.ToString(),
                    Bikewale.Utility.Image.GetPathToShowImages(thirdArticle.OriginalImgUrl, thirdArticle.HostUrl, Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(thirdArticle.OriginalImgUrl, thirdArticle.HostUrl, Bikewale.Utility.ImageSize._642x361),
                    thirdArticle.Description,
                    thirdArticle.Description,
                    relatedItem1,
                    relatedItem2,
                    relatedItem4
                    );
            }
            else
            {
                ArticleSummary forthArticle = articles.Skip(index).FirstOrDefault();
                AddRSSItem(
                    forthArticle.BasicId.ToString(),
                    forthArticle.Title,
                    String.Format("http://www.bikewale.com/news/{0}-{1}.html", forthArticle.BasicId, forthArticle.ArticleUrl),
                    forthArticle.AuthorName,
                    forthArticle.DisplayDate.ToString(),
                    forthArticle.Views.ToString(),
                    Bikewale.Utility.Image.GetPathToShowImages(forthArticle.OriginalImgUrl, forthArticle.HostUrl, Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(forthArticle.OriginalImgUrl, forthArticle.HostUrl, Bikewale.Utility.ImageSize._642x361),
                    forthArticle.Description,
                    forthArticle.Description,
                    relatedItem1,
                    relatedItem2,
                    relatedItem4
                    );
            }
        }

        /// <summary>
        /// Writes the beginning of a RSS document to an XmlTextWriter
        /// Modified By : Sadhana Upadhyay on 4th Apr 2014
        /// Summary : To add img:alt tag
        /// </summary>                      
        public void WriteRSSPrologue()
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");
            writer.WriteAttributeString("xmlns:bw", "http://www.bikewale.com/cwChannelModule");
            writer.WriteAttributeString("xmlns:img", "http://www.bikewale.com/cwChannelModule");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", "Bike News - Latest Indian Bike News & Views | BikeWale");
            writer.WriteElementString("img:alt", "Bike News - Latest Indian Bike News & Views | BikeWale");
            writer.WriteElementString("link", "http://www.bikewale.com/news/");
            writer.WriteElementString("description", "Latest news updates on Indian bike industry, expert views and interviews exclusively on BikeWale.");
            writer.WriteElementString("copyright", "Copyright " + DateTime.Now.Year + " BikeWale");
            writer.WriteElementString("generator", "BikeWale RSS Generator");
            writer.WriteStartElement("atom:link");
            writer.WriteAttributeString("href", "http://www.bikewale.com/news/feed/");
            writer.WriteAttributeString("rel", "self");
            writer.WriteAttributeString("type", "application/rss+xml");
            writer.WriteEndElement();
        }

        /// <summary>
        /// Adds a single item to the XmlTextWriter supplied
        /// Modified By : Sadhana Upadhyay on 4th Apr 2014
        /// Summary : To add img:alt tag
        /// </summary>
        /// <param name="sItemId">The Id of the RSS item</param>
        /// <param name="sItemTitle">The title of the RSS item</param>
        /// <param name="sItemAuthor">The Author of the RSS item</param>
        /// <param name="sItemDisplayDate">The Date the RSS item is displayed</param>
        /// <param name="sItemViews">The Number of view for the RSS item</param>
        /// <param name="sItemThumbImg">Thumbnail image of the RSS item</param>
        /// <param name="sItemLargeImg">Large image of the RSS item</param>
        /// <param name="sItemDescription">The description of the RSS item</param>
        /// <param name="sItemContent">The content of the RSS item</param>           
        public void AddRSSItem(string sItemId, string sItemTitle, string sItemLink, string sItemAuthor, string sItemDisplayDate, string sItemViews, string sItemThumbImg,
            string sItemLargeImg, string sItemDescription, string sItemContent, string relItem1, string relItem2, string relItem3)
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("bw:id", sItemId);
            writer.WriteStartElement("title");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();
            //writer.WriteElementString("title", sItemTitle);
            //writer.WriteElementString("img:alt", sItemTitle);

            writer.WriteElementString("link", sItemLink);
            writer.WriteElementString("guid", sItemLink);
            writer.WriteElementString("bw:author", sItemAuthor);
            writer.WriteElementString("bw:displayDate", sItemDisplayDate);
            writer.WriteElementString("bw:views", sItemViews);
            writer.WriteElementString("bw:thumbImg", sItemThumbImg);
            writer.WriteElementString("bw:largeImg", sItemLargeImg);
            writer.WriteStartElement("img:alt");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();
            writer.WriteElementString("bw:imgAttbn", "BikeWale");
            writer.WriteStartElement("img:imgCaption");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();
            //writer.WriteElementString("description", sItemDescription);
            writer.WriteStartElement("description");
            writer.WriteCData(sItemDescription);
            writer.WriteEndElement();
            writer.WriteStartElement("bw:content");
            writer.WriteCData(sItemContent);
            writer.WriteEndElement();
            writer.WriteElementString("bw:related1", relItem1);
            writer.WriteElementString("bw:related2", relItem2);
            writer.WriteElementString("bw:related3", relItem3);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Finishes up the XmlTextWriter by closing open elements and the document itself
        /// </summary>          
        public void WriteRSSClosing()
        {
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

    }   // End of class
}   // End of namespace