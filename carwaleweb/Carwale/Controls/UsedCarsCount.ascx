<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Carwale.UI.Controls.UsedCarsCount"  %>
 <div id="divUsedCarsCount" runat="server" class="mid-box float-lt">
    <span style="font-size: 13px;"><a href= "/used/<%= UrlRewrite.FormatSpecial(MakeName)%>-<%= (MaskingName)%>-cars/"  > <%=liveListingcount %>  Used  <%= MakeName +" " + ModelName  %> </a></span>   
    <span class="text-grey">at ₹ <%= minLiveListingPrice %> onwards</span>
</div>
