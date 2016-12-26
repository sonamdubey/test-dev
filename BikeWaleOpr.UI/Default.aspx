<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Default" Trace="false" Debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
    .card { width: 35%; font-size:17px; background-color: #737373; color:#fff; padding:15px; border-radius: 2px; line-height: 48px; -webkit-box-shadow: 4px 3px 5px 0px rgba(0,0,0,0.75);-moz-box-shadow: 4px 3px 5px 0px rgba(0,0,0,0.75);box-shadow: 4px 3px 5px 0px rgba(0,0,0,0.75); }
    .card-title { font-size:24px; font-weight:500;line-height: 48px;  }
    .card a { color:#ffab40; }
    .card a:hover { color:#ffab40; text-decoration:none; }
</style>
<h1 style="margin-top:10px;">Administrator Home <asp:Label ID="lblSummary" runat="server" /></h1>
<% if (isShownNotification && dataObj!= null)
   { %>
<div class="margin-top15">    
        <div class="card">
            <!-- Basic Card -->
            <div style="">
              <div class="card-content white-text">
                <span class="card-title">Notifications</span>
                <p>Model units sold data has been last updated on <%=  dataObj.LastUpdateDate.ToString("dd/MM/yyyy")%></p>
              </div>
              <div class="card-action">
                <a href="/content/bikeunitssold.aspx">Click here to Update data</a>
              </div>
            </div>
        </div>
</div>
<% } %>
<!-- #Include file="/includes/footerNew.aspx" -->