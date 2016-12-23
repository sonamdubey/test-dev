<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Default" Trace="false" Debug="false" %>

<!-- #Include file="/includes/headerNew.aspx" -->
    <h1 style="margin:0">Administrator Home <asp:Label ID="lblSummary" runat="server" /></h1>

<% if(isShownNotification) { %>
  <div  style="width: 450px;  margin-top:50px;">
            <fieldset>
                <legend>Notifications</legend>
            <p><h2>Models unit sold data has been last updated on <%= dataObj.LastUpdateDate %> .
                Please Update data.</h2>
            </p>
           </fieldset>
  </div>
<% } %>
<!-- #Include file="/includes/footerNew.aspx" -->