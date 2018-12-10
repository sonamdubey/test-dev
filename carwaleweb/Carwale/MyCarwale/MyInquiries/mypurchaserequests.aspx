<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInquiries.MyPurchaseRequests" AutoEventWireUp="false" trace="false" Debug="false" %>
<style type="text/css">
    #divRequests { margin-top:15px; }
    #divRequests { border-left:1px solid #ccc; border-top:1px solid #ccc; }
    #divRequests th { background-color:#ccc; }
    #divRequests th, #divRequests td { border-right:1px solid #ccc; border-bottom:1px solid #ccc; text-align:left; padding:5px 10px; }
</style>
<form runat="server">
    <div style="min-height:200px;">
        <div>
            <strong class="inline-block">Inquiries Received</strong>
            <asp:DropDownList Id="ddlRequestTime" runat="server" class="form-control inline-block" style="width:120px">
                <asp:ListItem Value="7">Last 7 Days</asp:ListItem>
                <asp:ListItem Value="0">Today</asp:ListItem>
                <asp:ListItem Value="1">Yesterday</asp:ListItem>
                <asp:ListItem Value="-1">All</asp:ListItem>
            </asp:DropDownList>
            <span id="errRequests" runat="server" class="text-red inline-block font12"></span>
        </div>
        <div id="divRequests" runat="server">
            <asp:Repeater ID="rptRequests" runat="server">
                <headertemplate>
                    <table id="tblRequests" border="0" cellspacing="0" cellpading="0" width="100%" >
                        <tr>
                            <th>Sr. No.</th>
                            <th>Name</th>
                            <th>Mobile</th>
                            <th>Email</th>
                            <th>Request Date</th>
                        </tr>                        
                </headertemplate>
                <itemtemplate>
                    <tr>
                        <td><%= (buyerCount++).ToString() %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "CustomerName") != null && !DataBinder.Eval(Container.DataItem, "CustomerName").ToString().Trim().ToLower().Equals("unknown") ? DataBinder.Eval(Container.DataItem, "CustomerName") : string.Empty %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "CustomerMobile") != null ? DataBinder.Eval(Container.DataItem, "CustomerMobile") : string.Empty %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "CustomerEmail") != null && !DataBinder.Eval(Container.DataItem, "CustomerEmail").ToString().Trim().ToLower().Contains("@unknown.com") && !String.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem, "CustomerEmail").ToString()) ? DataBinder.Eval(Container.DataItem, "CustomerEmail") : "-NA-" %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "RequestDateTime") %></td>
                    </tr>
                </itemtemplate>
                <footertemplate>
                    </table>
                </footertemplate>
            </asp:Repeater>
        </div>                    
    </div>
    <script type="text/javascript">
        totalPurchaseRequests = '<%= totalPurchaseRequests%>';
        $("#spnInquiryCount").text(" (" + totalPurchaseRequests + ")");
    </script>
</form>
