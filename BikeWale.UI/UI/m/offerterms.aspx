<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offerterms.aspx.cs" Inherits="Bikewale.Mobile.OfferTerms" %>

<!-- #include file="/UI/includes/headermobile.aspx" -->

<div class="padding5">
    <div class="grid_10 format-content margin-top10">
        <% if (isExpired)
           { %>
        <p class="margin-bottom5 font18 text-bold" style="color: red;">This offer has expired !</p>
        <% } %>
        <%=htmlContent%>
    </div>
</div>
<!-- #include file="/UI/includes/footermobile.aspx" -->
