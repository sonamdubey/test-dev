<%@ Control Language="C#" AutoEventWireUp="false"Inherits="Carwale.UI.Controls.ReviewRating" %>
<%@ Import Namespace="Carwale.UI.Common" %> 
<div id="reviewRating" runat="server">
    <asp:label id="lblImageRating" runat="server" />  <%= ReviewRate %>/5 based on <a href="/<%# UrlRewrite.FormatSpecial( Eval( "Make.MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial( Eval( "Model.ModelName").ToString()) %>/userreviews/"><%=ReviewCount %> reviews</a>
</div>
<div id="notRatedYet" runat="server" visible="false">
    Not rated yet, <a href= <%=Carwale.Utility.ManageCarUrl.CreateRatingPageUrl(Convert.ToInt32(ModelId))%>>Be the first one to write a review</a>
</div>
