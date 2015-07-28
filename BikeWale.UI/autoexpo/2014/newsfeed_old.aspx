<%@ Page Language="C#" ContentType="application/xml" AutoEventWireUp="false" Trace="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Xml" %>
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
        whereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 ";
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
                StartDocument();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CreateItem(i);
                }
                EndDocument();
            }
        }
        catch (Exception err)
        {
            ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            objErr.SendMail();
        }
    }

    public void WriteRSSPrologue()
    {
        writer.WriteStartDocument();
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteAttributeString("xmlns:blogChannel",
           "http://backend.userland.com/blogChannelModule");
        writer.WriteStartElement("channel");
        writer.WriteElementString("title", "Auto Expo 2014 News Feed");
        writer.WriteElementString("link", "http://autoexpo.carwale.com/2014/");
        writer.WriteElementString("description",
           "Auto Expo 2014 News Feed");
        writer.WriteElementString("copyright", "Copyright 2014 CarWale");
        writer.WriteElementString("generator", "RSSviaXmlTextWriter v1.0");        
    }
    
    protected void StartDocument()
    {
        writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
        writer.WriteStartDocument();
        //writer.WriteStartElement("items");
        writer.WriteStartElement("channel");
    }

    protected void EndDocument()
    {
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
    }

    protected void CreateItem(int index)
    {
        writer.WriteStartElement("item");

        writer.WriteElementString("id", ds.Tables[0].Rows[index]["BasicId"].ToString());

        writer.WriteStartElement("title");
        writer.WriteCData(ds.Tables[0].Rows[index]["Title"].ToString());
        writer.WriteEndElement();

        writer.WriteElementString("author", ds.Tables[0].Rows[index]["AuthorName"].ToString());
        writer.WriteElementString("displayDate", ds.Tables[0].Rows[index]["DisplayDate"].ToString());
        writer.WriteElementString("views", ds.Tables[0].Rows[index]["Views"].ToString());

        writer.WriteElementString("thumbImg", ImagingFunctions.GetImagePath("/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_m");
        writer.WriteElementString("largeImg", ImagingFunctions.GetImagePath("/ec/") + ds.Tables[0].Rows[index]["BasicId"].ToString() + "/img/m/" + ds.Tables[0].Rows[index]["BasicId"].ToString() + "_l");

        writer.WriteStartElement("description");
        writer.WriteCData(ds.Tables[0].Rows[index]["Description"].ToString());
        writer.WriteEndElement();

        writer.WriteStartElement("content");
        writer.WriteCData(ds.Tables[0].Rows[index]["Content"].ToString());
        writer.WriteEndElement();

        writer.WriteEndElement();
    }
</script>