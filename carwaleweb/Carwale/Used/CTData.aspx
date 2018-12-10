<%@ Page Language="C#" AutoEventWireup="true" Trace="false" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Data.Common" %>
<%@ Import Namespace="MySql.Data.MySqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Carwale.DAL.CoreDAL.MySql" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        var dealerId = Request.QueryString["dealerId"];
        int intDealerId;
        if (!Int32.TryParse(dealerId, out intDealerId))
        {
            Response.Write("<span style='height:16px;Z-INDEX: 102; color : red;LEFT: 288px; POSITION: absolute; font-size: 200%; TOP: 144px'> Please pass valid dealerId.</span>");
        }
        string sql = @"SELECT s.DealerId, l.ProfileId, l.SellerType, s.TC_StockId AS 'CT_stockid', l.PhotoCount, s.EntryDate,
                              s.LastUpdated, l.VersionId, l.Price, l.Kilometers, l.AdditionalFuel, l.MakeYear, l.Color, l.Owners,
                              concat(l.HostURL, l.OriginalImgPath) AS 'CW_MainImg_url', l.BoostPackageId, l.BoostLeadThreshold 
                       FROM livelistings l INNER JOIN 
                            sellinquiries s ON l.Inquiryid = s.ID INNER JOIN 
                            cwmasterdb.dealers d ON d.ID = s.DealerId 
                       WHERE s.SourceId =1 AND l.SellerType = 1 AND l.DealerId = @dealerId ORDER BY l.EntryDate DESC";
        using (DbCommand cmd = DbFactory.GetDBCommand())
        {
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(DbFactory.GetDbParam("@dealerId", DbType.Int32, intDealerId));
            CTData1.DataSource = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ClassifiedMySqlReadConnection);
            CTData1.DataBind();
        }
    }
</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="CTData1" runat="server">

                <HeaderTemplate>
                    <table border="1" width="600px " style="text-align: center;">
                        <tr>
                            <th>DealerId</th>
                            <th>ProfileId</th>
                            <th>SellerType</th>
                            <th>CT_stockid</th>
                            <th>EntryDate</th>
                            <th>LastUpdated</th>
                            <th>VersionId</th>
                            <th>Price</th>
                            <th>Kilometers</th>
                            <th>AdditionalFuel</th>
                            <th>MakeYear</th>
                            <th>Color</th>
                            <th>Owners</th>
                            <th>PhotoCount</th>
                            <th>CW_MainImg_url</th>
                            <th>BoostPackageId</th>
                            <th>BoostLeadThreshold</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="100">
                            <%# DataBinder.Eval(Container, "DataItem.DealerId")%>
                        </td>
                        <td width="100">
                            <a href="http://www.carwale.com/used/cardetails.aspx?car=<%# DataBinder.Eval(Container, "DataItem.ProfileId")%>"><%# DataBinder.Eval(Container, "DataItem.ProfileId")%></a>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.SellerType")%>
                        </td>
                        <td width="150">
                            <%# DataBinder.Eval(Container, "DataItem.CT_stockid")%>
                        </td>
                        <td width="100" align="center">
                            <%# DataBinder.Eval(Container, "DataItem.EntryDate")%>
                        </td>
                        <td width="100">
                            <%# DataBinder.Eval(Container, "DataItem.LastUpdated")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.VersionId")%>
                        </td>
                        <td width="150">
                            <%# DataBinder.Eval(Container, "DataItem.Price")%>
                        </td>
                        <td width="100" align="center">
                            <%# DataBinder.Eval(Container, "DataItem.Kilometers")%>
                        </td>
                        <td width="100">
                            <%# DataBinder.Eval(Container, "DataItem.AdditionalFuel")%>
                        </td>
                        <td width="150">
                            <%# DataBinder.Eval(Container, "DataItem.MakeYear")%>
                        </td>
                        <td width="100" align="center">
                            <%# DataBinder.Eval(Container, "DataItem.Color")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.Owners")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.PhotoCount")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.CW_MainImg_url")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.BoostPackageId")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container, "DataItem.BoostLeadThreshold")%>
                        </td>
                    </tr>
                </ItemTemplate>

            </asp:Repeater>

        </div>
    </form>
</body>
</html>
