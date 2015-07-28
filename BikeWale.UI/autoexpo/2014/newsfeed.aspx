<%@ Page Language="C#" ContentType="application/xml" AutoEventWireUp="false" Trace="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.IO" %>
<script runat="server">
    private int CurrentPageIndex = 1, PageSize = 10;
    DataSet ds = new DataSet();
    
    XmlTextWriter writer;
    protected override void OnInit(EventArgs e)
    {        
        this.Load += new EventHandler(Page_Load);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        writer = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);
        if (Request["pn"] != null && Request.QueryString["pn"].ToString().Trim() != string.Empty)
        {
            if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                CurrentPageIndex = Convert.ToInt32(Request.QueryString["pn"]);
        }
        GetNews();
    }

    private void GetNews()
    {
        SqlCommand cmd = new SqlCommand();
        string selectClause = string.Empty, fromClause = string.Empty, whereClause = string.Empty, orderByClause = string.Empty;
        selectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet ";
        selectClause = selectClause + ", (Select top 1 CPC.Data From Con_EditCms_Pages AS CP, Con_EditCms_PageContent AS CPC WHERE CP.BasicId = CB.Id and CPC.PageId = CP.Id) as Content";
        fromClause = " Con_EditCms_Basic AS CB ";
        whereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND YEAR(CB.PublishedDate) >= 2013";
        orderByClause = " DisplayDate Desc ";

        cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "9";

        try
        {
            int startIndex = (CurrentPageIndex - 1) * this.PageSize + 1;
            int endIndex = CurrentPageIndex * this.PageSize;

            string sql = " Select * From (Select Top " + endIndex + " Row_Number() Over (Order By " + orderByClause + ") AS RowN, "
                + " " + selectClause + " From " + fromClause + " "
                + (whereClause != "" ? " Where " + whereClause + " " : "")
                + " ) AS TopRecords Where "
                + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            //Response.Write(sql);

            CommonOpn objCom = new CommonOpn();
            Database db = new Database();
            ds = db.SelectAdaptQry(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                WriteRSSPrologue();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CreateItem(i);
                }
                WriteRSSClosing();

                writer.Flush();
                writer.Close();

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/xml";
                Response.Cache.SetCacheability(HttpCacheability.Public);

                Response.End();
            }
        }
        catch (Exception err)
        {
            ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            objErr.SendMail();
        }        
    }
    /// <summary>
    /// Creates the items in the RSS documents in the Channel element.
    /// </summary>
    /// <param name="index">row number from which value is retrieved from the dataset</param>
    protected void CreateItem(int index)
    {
        AddRSSItem(
            ds.Tables[0].Rows[index]["BasicId"].ToString(),
            ds.Tables[0].Rows[index]["Title"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
            "http://autoexpo.carwale.com/2014/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "-" + ds.Tables[0].Rows[index]["Url"].ToString() + ".html",
            ds.Tables[0].Rows[index]["AuthorName"].ToString(),
            ds.Tables[0].Rows[index]["DisplayDate"].ToString(),
            ds.Tables[0].Rows[index]["Views"].ToString(),
            ImagingFunctions.GetImagePath("/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_m",
            ImagingFunctions.GetImagePath("/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_l",
            ds.Tables[0].Rows[index]["Description"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""),
            ds.Tables[0].Rows[index]["Content"].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", "")
            );
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
        writer.WriteAttributeString("xmlns:cw", "http://autoexpo.carwale.com/cwChannelModule");
        writer.WriteStartElement("channel");
        writer.WriteElementString("title", "Auto Expo 2014 News Feed");
        writer.WriteElementString("link", "http://autoexpo.carwale.com/2014/");
        writer.WriteElementString("description", "Latest News from Auto Expo 2014");
        writer.WriteElementString("copyright", "Copyright 2014 CarWale");
        writer.WriteElementString("generator", "CarWale RSS Generator");
        writer.WriteStartElement("atom:link");
        writer.WriteAttributeString("href", "http://autoexpo.carwale.com/2014/feed/");
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
        string sItemLargeImg, string sItemDescription, string sItemContent)
    {
        writer.WriteStartElement("item");
        writer.WriteElementString("cw:id", sItemId);
        writer.WriteStartElement("title");
        writer.WriteCData(sItemTitle);
        writer.WriteEndElement();
        //writer.WriteElementString("title", sItemTitle);
        writer.WriteElementString("link", sItemLink);
        writer.WriteElementString("guid", sItemLink);
        writer.WriteElementString("cw:author", sItemAuthor);
        writer.WriteElementString("cw:displayDate", sItemDisplayDate);
        writer.WriteElementString("cw:views", sItemViews);
        writer.WriteElementString("cw:thumbImg", sItemThumbImg);
        writer.WriteElementString("cw:largeImg", sItemLargeImg);
        //writer.WriteElementString("description", sItemDescription);
        writer.WriteStartElement("description");
        writer.WriteCData(sItemDescription);
        writer.WriteEndElement();
        writer.WriteStartElement("cw:content");
        writer.WriteCData(sItemContent);
        writer.WriteEndElement();
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
</script>