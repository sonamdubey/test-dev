<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Default" Trace="false" Debug="false" %>
<link rel="stylesheet" href="http://www.w3schools.com/lib/w3.css">

<!-- #Include file="/includes/headerNew.aspx" -->
    <h1 style="margin:0">Administrator Home <asp:Label ID="lblSummary" runat="server" /></h1>


<% if(isShownNotification) { %>
<div margin-top:20px;"> 
     <div class="w3-panel w3-card w3-yellow" >
            <fieldset>
                <legend>Notifications</legend>
            <p><h3>Models unit sold data has been last updated on <%=  dataObj.LastUpdateDate.ToString("dd/MM/yyyy")%> .
                Please Update data.</h3>
            </p>
           </fieldset>
          </div>
</div>

<% } %>
<!-- #Include file="/includes/footerNew.aspx" -->