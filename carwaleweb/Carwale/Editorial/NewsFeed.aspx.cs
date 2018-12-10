using Carwale.Entity.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications.Logs;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Carwale.UI.News
{
    public class NewsFeed : Page
    {
        protected Repeater rptNews;
        private string relatedItem1 = "", relatedItem2 = "", relatedItem3 = "", relatedItem4 = "";
        private int CurrentPageIndex = 1, PageSize = 10;
        private bool isCarwale = true;
        private string _slug = string.Empty, ApplicationId = "1";
        private string _subCat = string.Empty;
		private string utmSource = "yahoo";
		string footer = string.Empty;
        XmlTextWriter writer;


        protected string _title = string.Empty;
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
			if (Request["utmSource"] != null && Request.QueryString["utmSource"].ToString().Trim() != string.Empty)
			{ 
				utmSource = Request.QueryString["utmSource"].ToString();
			}
            if (Request["slug"] != null && Request.QueryString["slug"].ToString().Trim() != string.Empty)
            {
                _slug = Request.QueryString["slug"].ToString();
            }
            if (Request["cat"] != null && Request.QueryString["cat"].ToString().Trim() != string.Empty)
            {
                _subCat = Request.QueryString["cat"].ToString();
            }
            if (Request["application"] != null && Request.QueryString["application"].ToString().Trim() != string.Empty)
            {
                ApplicationId = Request.QueryString["application"].ToString();
                isCarwale = ApplicationId == "1";
            }

            footer = "<p>For more <a href=\"https://www.carwale.com/news/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">news</a>,<a href=\"https://www.carwale.com/expert-reviews/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">reviews</a>,<a href=\"https://www.carwale.com/videos/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">videos</a> and information about cars, visit <a href=\"https://www.carwale.com/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\">CarWale.com</a>.</p><p><a href=\"https://www.carwale.com/new/prices.aspx?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Check On-Road Prices</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/new/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Find New Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/upcoming-cars/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Upcoming Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/comparecars/?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Compare Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/new/locatenewcardealers.aspx?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Dealer Locator</a><p>";

            BindNews();

        }

        private List<Entity.CMS.Articles.ContentFeedSummary> GetContentFeeds()
        {
            List<Entity.CMS.Articles.ContentFeedSummary> newsFeeds = new List<Entity.CMS.Articles.ContentFeedSummary>();
            try
            {
                var cmsContent = UnityBootstrapper.Resolve<ICMSContent>();
                int startIndex = (CurrentPageIndex - 1) * this.PageSize + 1;
                int endIndex = CurrentPageIndex * this.PageSize;

                if (_slug != string.Empty && _subCat == string.Empty)
                {
                    newsFeeds = cmsContent.GetNewsFeedBySlug(new Entity.CMS.URIs.ContentFeedURI()
                    {
                        ApplicationId = (int)(isCarwale ? Carwale.Entity.Enum.Application.CarWale : Carwale.Entity.Enum.Application.BikeWale),
                        Slug = _slug,
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    });
                }
                else if (_slug == string.Empty && _subCat != string.Empty)
                {
                    newsFeeds = cmsContent.GetNewsFeedBySubCategory(new Entity.CMS.URIs.ContentFeedURI()
                    {
                        ApplicationId = (int)(isCarwale ? Carwale.Entity.Enum.Application.CarWale : Carwale.Entity.Enum.Application.BikeWale),
                        SubCategoryId = Convert.ToInt32(_subCat),
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    });
                }
                else
                {
                    newsFeeds = cmsContent.GetAllNewsFeed(new Entity.CMS.URIs.ContentFeedURI()
                    {
                        ApplicationId = (int)(isCarwale ? Carwale.Entity.Enum.Application.CarWale : Carwale.Entity.Enum.Application.BikeWale),
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    });
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return newsFeeds;
        }


        private void BindNews()
        {
            bool hasRows = false;
            try
            {
                var contentFeed = GetContentFeeds();
                if (contentFeed.Count > 0)
                {
                    hasRows = true;
                    WriteRSSPrologue();
                    GetRelatedItems(contentFeed);
                    int index = 0;
                    foreach (var feed in contentFeed)
                    {
                        CreateItem(index++, feed);
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
                Logger.LogException(err);
            }
            finally
            {
                if (writer.WriteState != WriteState.Closed)
                {
                    writer.Close();
                }
            }

            if (!hasRows)
                Response.Redirect("/news/");

            Response.End();
        }

        private void GetRelatedItems(List<Entity.CMS.Articles.ContentFeedSummary> data)
        {
            if (CurrentPageIndex == 1)
            {

                relatedItem1 = (isCarwale ? "https://www.carwale.com" + data[0].Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data[0].BasicId.ToString(), data[0].Url));
                if (data.Count > 1)
                {
                    relatedItem2 = (isCarwale ? "https://www.carwale.com" + data[1].Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data[1].BasicId.ToString(), data[1].Url)); 
                    relatedItem3 = (isCarwale ? "https://www.carwale.com" + data[2].Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data[2].BasicId.ToString(), data[2].Url)); 
                    relatedItem4 = (isCarwale ? "https://www.carwale.com" + data[3].Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data[3].BasicId.ToString(), data[3].Url)); 
                }
            }
            else
            {
                var cmscontent = UnityBootstrapper.Resolve<ICMSContent>();
                var relatedNews = cmscontent.GetAllNewsFeed(new Entity.CMS.URIs.ContentFeedURI()
                {
                    ApplicationId = (int)Carwale.Entity.Enum.Application.CarWale,
                    StartIndex = 1,
                    EndIndex = 4
                });
                if (relatedNews.Count > 0)
                {
                    relatedItem1 = (isCarwale ? "https://www.carwale.com" + relatedNews[0].Url.ToString() : string.Format("https://www.bikewale.com/news/{0}-{1}.html", relatedNews[0].BasicId.ToString(), relatedNews[0].Url));

                    if (relatedNews.Count > 1)
                    {
                        relatedItem2 = (isCarwale ? "https://www.carwale.com" + relatedNews[1].Url.ToString() : string.Format("https://www.bikewale.com/news/{0}-{1}.html", relatedNews[1].BasicId.ToString(), relatedNews[1].Url));
                        relatedItem3 = (isCarwale ? "https://www.carwale.com" + relatedNews[2].Url.ToString() : string.Format("https://www.bikewale.com/news/{0}-{1}.html", relatedNews[2].BasicId.ToString(), relatedNews[2].Url));
                        relatedItem4 = (isCarwale ? "https://www.carwale.com" + relatedNews[3].Url.ToString() : string.Format("https://www.bikewale.com/news/{0}-{1}.html", relatedNews[3].BasicId.ToString(), relatedNews[3].Url));
                    }
                }
            }
        }
        /// <summary>
        /// Creates the items in the RSS documents in the Channel element.
        /// </summary>
        /// <param name="index">row number from which value is retrieved from the dataset</param>
        protected void CreateItem(int index, Entity.CMS.Articles.ContentFeedSummary data)
        {
            if (index == 0)
            {
                AddRSSItem(
                    data.BasicId.ToString(),
                    data.Title.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    (isCarwale ? "https://www.carwale.com" + data.Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data.BasicId.ToString(), data.Url)),
                    data.AuthorName,
                    data.DisplayDate,
                    data.Views.ToString(),
                    !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._160X89 + data.OriginalImgPath : "",
                    !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._600X337 + data.OriginalImgPath : "",
                    data.Description.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    data.Content.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    relatedItem2,
                    relatedItem3,
                    relatedItem4
                    );
            }
            else if (index == 1)
            {
                AddRSSItem(
                   data.BasicId.ToString(),
                   data.Title.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   (isCarwale ? "https://www.carwale.com" + data.Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data.BasicId.ToString(), data.Url)),
                   data.AuthorName,
                   data.DisplayDate,
                   data.Views.ToString(),
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._160X89 + data.OriginalImgPath : "",
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._600X337 + data.OriginalImgPath : "",
                   data.Description.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   data.Content.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   relatedItem1,
                   relatedItem3,
                   relatedItem4
                   );
            }
            else if (index == 2)
            {
                AddRSSItem(
                   data.BasicId.ToString(),
                   data.Title.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   (isCarwale ? "https://www.carwale.com" + data.Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data.BasicId.ToString(), data.Url)),
                   data.AuthorName,
                   data.DisplayDate,
                   data.Views.ToString(),
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._160X89 + data.OriginalImgPath : "",
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._600X337 + data.OriginalImgPath : "",
                   data.Description.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   data.Content.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   relatedItem1,
                   relatedItem2,
                   relatedItem4
                   );
            }
            else
            {
                AddRSSItem(
                   data.BasicId.ToString(),
                   data.Title.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   (isCarwale ? "https://www.carwale.com" + data.Url : string.Format("https://www.bikewale.com/news/{0}-{1}.html", data.BasicId.ToString(), data.Url)),
                   data.AuthorName,
                   data.DisplayDate,
                   data.Views.ToString(),
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._160X89 + data.OriginalImgPath : "",
                   !String.IsNullOrEmpty(data.OriginalImgPath) ? data.HostUrl + ImageSizes._600X337 + data.OriginalImgPath : "",
                   data.Description.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   data.Content.Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                   relatedItem1,
                   relatedItem2,
                   relatedItem3
                   );

            }
        }

        /// <summary>
        /// Writes the beginning of a RSS document to an XmlTextWriter
        /// </summary>                      
        public void WriteRSSPrologue()
        {
            writer.WriteStartDocument();
            //writer.WriteComment("RSS generated by CarWale at " + DateTime.Now.ToString("r"));
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");
            writer.WriteAttributeString(isCarwale ? "xmlns:cw" : "xmlns:bw", "https://www." + (isCarwale ? "carwale" : "bikewale") + ".com/cwChannelModule");
            writer.WriteAttributeString("xmlns:img", "https://www." + (isCarwale ? "carwale" : "bikewale") + ".com/cwChannelModule");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", isCarwale ? "Car News - Latest Indian Car News & Views | CarWale" : "Bike News - Latest Indian Bike News & Views | BikeWale");
            writer.WriteElementString("img:alt", isCarwale ? "Car News - Latest Indian Car News & Views | CarWale" : "Bike News - Latest Indian Bike News & Views | BikeWale");
            writer.WriteElementString("link", "https://www." + (isCarwale ? "carwale" : "bikewale") + ".com/news/");
            writer.WriteElementString("description", isCarwale ? "Latest news updates on Indian car industry, expert views and interviews exclusively on CarWale." : "Latest news updates on Indian bike industry, expert views and interviews exclusively on BikeWale.");
            writer.WriteElementString("copyright", "Copyright " + (isCarwale ? "2012 CarWale" : "2016 BikeWale"));
            writer.WriteElementString("generator", (isCarwale ? "CarWale" : "BikeWale") + " RSS Generator");
            writer.WriteStartElement("atom:link");
            writer.WriteAttributeString("href", "https://www." + (isCarwale ? "carwale" : "bikewale") + ".com/news/feed/");
            writer.WriteAttributeString("rel", "self");
            writer.WriteAttributeString("type", "application/rss+xml");
            writer.WriteEndElement();
        }

        /// <summary>
        /// Adds a single item to the XmlTextWriter supplied
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
            string sItemLargeImg, string sItemDescription, string sItemContent, string sRelatedLink1, string sRelatedLink2, string sRelatedLink3)
        {
            string utmQS = isCarwale ? "?utm_source="+utmSource+"&utm_medium=sponsorship&utm_content=article_link&utm_campaign=allaboutcars" : "?utm_source=msn&utm_medium=sponsorship&utm_content=article_link&utm_campaign=allaboutbikes";
            writer.WriteStartElement("item");
            writer.WriteElementString(isCarwale ? "cw:id" : "bw:id", sItemId);
            writer.WriteStartElement("title");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();
            //writer.WriteElementString("title", sItemTitle);
            writer.WriteElementString("link", sItemLink + utmQS);
            writer.WriteElementString("guid", sItemLink + utmQS);
            writer.WriteElementString(isCarwale ? "cw:author" : "bw:author", sItemAuthor);
            writer.WriteElementString(isCarwale ? "cw:displayDate" : "bw:displayDate", sItemDisplayDate);
            writer.WriteElementString(isCarwale ? "cw:views" : "bw:views", sItemViews);
            writer.WriteElementString(isCarwale ? "cw:thumbImg" : "bw:thumbImg", sItemThumbImg);
            writer.WriteElementString(isCarwale ? "cw:largeImg" : "bw:largeImg", sItemLargeImg);

            writer.WriteStartElement(isCarwale ? "cw:imgAlt" : "bw:imgAlt");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();

            writer.WriteElementString(isCarwale ? "cw:imgAttbn" : "bw:imgAttbn", isCarwale ? "CarWale" : "BikeWale");
            writer.WriteStartElement(isCarwale ? "cw:imgCaption" : "bw:imgCaption");
            writer.WriteCData(sItemTitle);
            writer.WriteEndElement();
            writer.WriteStartElement("description");
            writer.WriteCData(sItemDescription);
            writer.WriteEndElement();
            writer.WriteStartElement(isCarwale ? "cw:content" : "bw:content");
            //Adding the hardcoded footer
            //sItemContent+="<p>For more <a href=\"https://www.carwale.com/news/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">news</a>,<a href=\"https://www.carwale.com/expert-reviews/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">reviews</a>,<a href=\"https://www.carwale.com/videos/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">videos</a> and information about cars, visit <a href=\"https://www.carwale.com/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\">CarWale.com</a>.</p><p><a href=\"https://www.carwale.com/new/prices.aspx?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Check On-Road Prices</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/new/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Find New Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/upcoming-cars/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Upcoming Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/comparecars/?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Compare Cars</a>&nbsp;|&nbsp;<a href=\"https://www.carwale.com/new/locatenewcardealers.aspx?utm_source=yahoo&utm_medium=sponsorship&utm_content=footer_link&utm_campaign=allaboutcars\" target=\"_blank\">Dealer Locator</a><p>";
            writer.WriteCData(sItemContent + (isCarwale ? footer:string.Empty));
            writer.WriteEndElement();
            writer.WriteElementString(isCarwale ? "cw:related1" : "bw:related1", sRelatedLink1 + utmQS);
            writer.WriteElementString(isCarwale ? "cw:related2" : "bw:related2", sRelatedLink2 + utmQS);
            writer.WriteElementString(isCarwale ? "cw:related3" : "bw:related3", sRelatedLink3 + utmQS);

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
    } // class
} // namespace