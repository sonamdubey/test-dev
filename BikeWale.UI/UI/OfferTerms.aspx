<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offerterms.aspx.cs" Inherits="Bikewale.OfferTerms" %>
<% 
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd600x270Shown = false;
%>
<!-- #include file="/UI/includes/headhome.aspx" -->


<div class="container_12">
    <div class="grid_10 format-content margin-top10">
        <% if (isExpired)
           { %>
        <p class="margin-bottom5 font18 text-bold" style="color: red;">This offer has expired !</p>
        <% } %>
        <%=htmlContent%>
    </div>
</div>
<!-- #include file="/UI/includes/footerinner.aspx" -->
