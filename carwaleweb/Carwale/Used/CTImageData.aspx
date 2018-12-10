<%@ Page Language="C#" AutoEventWireup="true"  %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!this.IsPostBack)
        {
            
            //Populating a DataTable from database.
            System.Data.DataTable dt = this.GetData();

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            //Table start.
            html.Append("<table border = '1'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (System.Data.DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (System.Data.DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
        }
    }

    private System.Data.DataTable GetData()
    {
        string stockId = Request.QueryString["stockId"];
        string sqlQ = @"select CP.InquiryId,CP.Description,CP.IsDealer,CP.IsMain,CP.IsActive,CP.Entrydate,CP.TC_CarPhotoId,concat(CP.HostURL,'0x0',CP.OriginalImgPath) as URL,Si.SourceId from carphotos CP INNER JOIN sellinquiries Si on Si.Id=CP.InquiryId where Si.SourceId = 1 AND Si.TC_StockId = @stockId AND Si.statusid = 1";
        using (System.Data.Common.DbCommand cmd = Carwale.DAL.CoreDAL.MySql.DbFactory.GetDBCommand())
        {
            cmd.CommandText = sqlQ;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add(Carwale.DAL.CoreDAL.MySql.DbFactory.GetDbParam("@stockId", System.Data.DbType.Int32, stockId));
            System.Data.DataTable dt = Carwale.DAL.CoreDAL.MySql.MySqlDatabase.SelectAdapterQuery(cmd, Carwale.DAL.CoreDAL.MySql.DbConnections.ClassifiedMySqlReadConnection).Tables[0];
            return dt;
        }
    }
</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
</body>
</html>
