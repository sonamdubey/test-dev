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
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
namespace Bikewale.News
{
    public class Sitemap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateNewsSiteMap();
        }
        private void GenerateNewsSiteMap()
        {
            string mydomain = "http://www.bikewale.com/news/";

            XmlTextWriter writer = null;
            CMSContent objNews = null;
            IEnumerable<ArticleSummary> articles = null;
            DateTime twoDaysOld = DateTime.Now.AddDays(-2).Date;
            try
            {
                writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
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
                // Creating the SiteMap XML using XMLTextWriter
                writer.Formatting = System.Xml.Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                writer.WriteAttributeString("xmlns", "news", null, "http://www.google.com/schemas/sitemap-news/0.9");
                writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                if (objNews != null && objNews.RecordCount > 0)
                {
                    articles = objNews.Articles;
                    articles = from article in articles
                               where article.DisplayDate.Date >= twoDaysOld
                               orderby article.DisplayDate descending
                               select article;

                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", String.Format("{0}{1}-{2}.html", mydomain, article.BasicId, article.ArticleUrl));
                            writer.WriteStartElement("news:news");
                            writer.WriteStartElement("news:publication");
                            writer.WriteElementString("news:name", "BikeWale");
                            writer.WriteElementString("news:language", "en");
                            writer.WriteEndElement();
                            writer.WriteElementString("news:genres", "PressRelease, Blog");
                            writer.WriteElementString("news:publication_date", article.DisplayDate.ToString("yyyy-MM-ddThh:mm:sszzz"));
                            //writer.WriteElementString("news:keywords", article.tTag"]) ? "" : dtr["Tag"].ToString());
                            writer.WriteElementString("news:title", article.Title);
                            writer.WriteEndElement();
                            if (!String.IsNullOrEmpty(article.HostUrl))
                            {
                                writer.WriteStartElement("image:image");
                                writer.WriteElementString("image:loc", string.Format("{0},{1},{2}", article.HostUrl, ImageSize._174x98, article.OriginalImgUrl));
                                //writer.WriteElementString("image:title", dtr["Caption"].ToString());
                                //writer.WriteElementString("image:caption", dtr["Caption"].ToString());
                                writer.WriteElementString("image:geo_location", "India");
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                    }
                }
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/xml";
                Response.Cache.SetCacheability(HttpCacheability.Public);
            }

            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }//class
}//namespace