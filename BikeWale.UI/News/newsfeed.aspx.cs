using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;

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
            //bool hasRows = false;

            //if (_slug != string.Empty && _subCat == string.Empty)
            //{
            //    selectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet, I.HostUrl, I.ImagePathLarge, I.ImagePathThumbnail,I.OriginalImgPath ";
            //    selectClause = selectClause + ", (Select top 1 CPC.Data From Con_EditCms_Pages AS CP, Con_EditCms_PageContent AS CPC With(NoLock) WHERE CP.BasicId = CB.Id and CPC.PageId = CP.Id) as Content";
            //    fromClause = " Con_EditCms_Tags CT With(NoLock) "
            //            + " Inner Join Con_EditCms_BasicTags BT With(NoLock) On BT.TagId = CT.Id "
            //            + " Inner Join Con_EditCMs_Basic CB With(NoLock) On CB.Id = BT.BasicId "
            //            + " LEFT JOIN Con_EditCms_Images AS I With(NoLock) ON I.BasicId = CB.Id AND I.IsMainImage = 1 ";
            //    whereClause = " CT.Slug = @Slug And CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationId = 2 ";
            //    orderByClause = " DisplayDate Desc ";

            //    cmd.Parameters.Add("@Slug", SqlDbType.VarChar, 150).Value = _slug;
            //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "1";
            //}
            //else if (_slug == string.Empty && _subCat != string.Empty)
            //{
            //    selectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet ,  I.HostUrl, I.ImagePathLarge, I.ImagePathThumbnail,I.OriginalImgPath ";
            //    selectClause = selectClause + ", (Select top 1 CPC.Data From Con_EditCms_Pages AS CP, Con_EditCms_PageContent AS CPC With(NoLock) WHERE CP.BasicId = CB.Id and CPC.PageId = CP.Id) as Content";
            //    fromClause = " Con_EditCms_SubCategories SC With(NoLock) "
            //                + " Inner Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.SubCategoryId = SC.Id "
            //                + " Inner Join Con_EditCMs_Basic CB With(NoLock) On CB.Id = BSC.BasicId "
            //                + " LEFT JOIN Con_EditCms_Images AS I With(NoLock) ON I.BasicId = CB.Id AND I.IsMainImage = 1 ";
            //    whereClause = " SC.Id = @SubCatId And CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationId = 2 ";
            //    orderByClause = " DisplayDate Desc ";

            //    cmd.Parameters.Add("@SubCatId", SqlDbType.BigInt).Value = _subCat;
            //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "1";
            //}
            //else
            //{
            //    selectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet,  I.HostUrl, I.ImagePathLarge, I.ImagePathThumbnail,I.OriginalImgPath ";
            //    selectClause = selectClause + ", (Select top 1 CPC.Data From Con_EditCms_Pages AS CP, Con_EditCms_PageContent AS CPC With(NoLock) WHERE CP.BasicId = CB.Id and CPC.PageId = CP.Id) as Content";
            //    fromClause = " Con_EditCms_Basic AS CB With(NoLock) "
            //               + " LEFT JOIN Con_EditCms_Images AS I With(NoLock) ON I.BasicId = CB.Id AND I.IsMainImage = 1 ";
            //    whereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationId = 2 ";
            //    orderByClause = " DisplayDate Desc ";

            //    cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "1";
            //}

            //try
            //{
            //    int startIndex = (CurrentPageIndex - 1) * this.PageSize + 1;
            //    int endIndex = CurrentPageIndex * this.PageSize;

            //    string sql = " Select * From (Select Top " + endIndex + " Row_Number() Over (Order By " + orderByClause + ") AS RowN, "
            //        + " " + selectClause + " From " + fromClause + " "
            //        + (whereClause != "" ? " Where " + whereClause + " " : "")
            //        + " ) AS TopRecords Where "
            //        + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";

            //    Trace.Warn(sql);
            //    cmd.CommandText = sql;
            //    cmd.CommandType = CommandType.Text;

            //    //Response.Write(sql);

            //    CommonOpn objCom = new CommonOpn();

            //    cwConnectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            //    Database db = new Database(cwConnectionString);
            //    ds = db.SelectAdaptQry(cmd);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        hasRows = true;
            //        WriteRSSPrologue();
            //        GetRelatedItems();

            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            CreateItem(i);
            //        }
            //        WriteRSSClosing();

            //        writer.Flush();
            //        writer.Close();

            //        Response.ContentEncoding = System.Text.Encoding.UTF8;
            //        Response.ContentType = "text/xml";
            //        Response.Cache.SetCacheability(HttpCacheability.Public);
            //    }
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}

            //if (!hasRows)
            //{
            //    Response.Redirect("/news/", false);
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //    this.Page.Visible = false;
            //}

            //Response.End();
        }

        private void GetRelatedItems()
        {
            if (CurrentPageIndex == 1)
            {
                relatedItem1 = "http://www.bikewale.com/news/" + ds.Tables[0].Rows[0]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[0]["Url"].ToString() + ".html";
                relatedItem2 = "http://www.bikewale.com/news/" + ds.Tables[0].Rows[1]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[1]["Url"].ToString() + ".html";
                relatedItem3 = "http://www.bikewale.com/news/" + ds.Tables[0].Rows[2]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[2]["Url"].ToString() + ".html";
                relatedItem4 = "http://www.bikewale.com/news/" + ds.Tables[0].Rows[3]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[3]["Url"].ToString() + ".html";
            }
            else
            {
                GetRelatedLinksFromDB();
                if (dsRelated.Tables[0].Rows.Count > 0)
                {
                    relatedItem1 = "http://www.bikewale.com/news/" + dsRelated.Tables[0].Rows[0]["BasicId"].ToString() + "-" + dsRelated.Tables[0].Rows[0]["Url"].ToString() + ".html";
                    relatedItem2 = "http://www.bikewale.com/news/" + dsRelated.Tables[0].Rows[1]["BasicId"].ToString() + "-" + dsRelated.Tables[0].Rows[1]["Url"].ToString() + ".html";
                    relatedItem3 = "http://www.bikewale.com/news/" + dsRelated.Tables[0].Rows[2]["BasicId"].ToString() + "-" + dsRelated.Tables[0].Rows[2]["Url"].ToString() + ".html";
                    relatedItem4 = "http://www.bikewale.com/news/" + dsRelated.Tables[0].Rows[3]["BasicId"].ToString() + "-" + dsRelated.Tables[0].Rows[3]["Url"].ToString() + ".html";
                }
            }
        }

        private void GetRelatedLinksFromDB()
        {
            throw new Exception("Method not used/commented");

            //SqlCommand cmd = new SqlCommand();
            //int startIndex = 1;
            //int endIndex = 4;
            //string sql = " Select * From (Select Top " + endIndex + " Row_Number() Over (Order By " + orderByClause + ") AS RowN, "
            //        + " " + selectClause + " From " + fromClause + " "
            //        + (whereClause != "" ? " Where " + whereClause + " " : "")
            //        + " ) AS TopRecords Where "
            //        + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";
            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //CommonOpn objCom = new CommonOpn();

            //cwConnectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            //Database db = new Database(cwConnectionString);
            //dsRelated = db.SelectAdaptQry(cmd);

        }

        /// <summary>
        /// Creates the items in the RSS documents in the Channel element.
        /// </summary>
        /// <param name="index">row number from which value is retrieved from the dataset</param>
        protected void CreateItem(int index)
        {
            if (index == 0)
            {
                AddRSSItem(
                    ds.Tables[0].Rows[index]["BasicId"].ToString(),
                    ds.Tables[0].Rows[index]["Title"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    "http://www.bikewale.com/news/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[index]["Url"].ToString() + ".html",
                    ds.Tables[0].Rows[index]["AuthorName"].ToString(),
                    ds.Tables[0].Rows[index]["DisplayDate"].ToString(),
                    ds.Tables[0].Rows[index]["Views"].ToString(),
                    //Bikewale.Common.ImagingFunctions.GetImagePath("/bikewaleimg/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_m",
                    //Bikewale.Common.ImagingFunctions.GetImagePath("/bikewaleimg/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_l",
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._642x361),
                    ds.Tables[0].Rows[index]["Description"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    ds.Tables[0].Rows[index]["Content"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    relatedItem2,
                    relatedItem3,
                    relatedItem4
                    );
            }
            else if (index == 1)
            { 
                AddRSSItem(
                    ds.Tables[0].Rows[index]["BasicId"].ToString(),
                    ds.Tables[0].Rows[index]["Title"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    "http://www.bikewale.com/news/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[index]["Url"].ToString() + ".html",
                    ds.Tables[0].Rows[index]["AuthorName"].ToString(),
                    ds.Tables[0].Rows[index]["DisplayDate"].ToString(),
                    ds.Tables[0].Rows[index]["Views"].ToString(),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathThumbnail"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathLarge"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._642x361),
                    ds.Tables[0].Rows[index]["Description"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    ds.Tables[0].Rows[index]["Content"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    relatedItem1,
                    relatedItem3,
                    relatedItem4
                    );
            }
            else if (index == 2)
            {
                AddRSSItem(
                    ds.Tables[0].Rows[index]["BasicId"].ToString(),
                    ds.Tables[0].Rows[index]["Title"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    "http://www.bikewale.com/news/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[index]["Url"].ToString() + ".html",
                    ds.Tables[0].Rows[index]["AuthorName"].ToString(),
                    ds.Tables[0].Rows[index]["DisplayDate"].ToString(),
                    ds.Tables[0].Rows[index]["Views"].ToString(),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathThumbnail"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathLarge"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._642x361),
                    ds.Tables[0].Rows[index]["Description"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    ds.Tables[0].Rows[index]["Content"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    relatedItem1,
                    relatedItem2,
                    relatedItem4
                    );
            }
            else
            {
                AddRSSItem(
                    ds.Tables[0].Rows[index]["BasicId"].ToString(),
                    ds.Tables[0].Rows[index]["Title"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    "http://www.bikewale.com/news/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[index]["Url"].ToString() + ".html",
                    ds.Tables[0].Rows[index]["AuthorName"].ToString(),
                    ds.Tables[0].Rows[index]["DisplayDate"].ToString(),
                    ds.Tables[0].Rows[index]["Views"].ToString(),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathThumbnail"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    //Bikewale.Common.ImagingFunctions.GetPathToShowImages(ds.Tables[0].Rows[index]["ImagePathLarge"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString()),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._310x174),
                    Bikewale.Utility.Image.GetPathToShowImages(ds.Tables[0].Rows[index]["OriginalImgPath"].ToString(), ds.Tables[0].Rows[index]["HostUrl"].ToString(), Bikewale.Utility.ImageSize._642x361),
                    ds.Tables[0].Rows[index]["Description"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    ds.Tables[0].Rows[index]["Content"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
                    relatedItem1,
                    relatedItem2,
                    relatedItem3
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
            writer.WriteElementString("copyright", "Copyright 2012 BikeWale");
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