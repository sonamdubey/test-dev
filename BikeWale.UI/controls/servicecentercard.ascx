<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceCenterCard" %>
<% if(ServiceCenteList!=null) { %>

<div class="carousel-heading-content padding-top20">
    <div class="swiper-heading-left-grid inline-block">
        <h2><%=widgetHeading %></h2>
    </div><div class="swiper-heading-right-grid inline-block text-right">
        <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName) %>" title="<%= makeName %> Service Centers in <%=cityName %>" class="btn view-all-target-btn">View all</a>
    </div>
    <div class="clear"></div>
</div>
<%if(!string.IsNullOrEmpty(biLineText)) {%>
<p class="font14 margin-left20 margin-bottom20"><%=biLineText%></p>
<%} %>
<ul class="bw-horizontal-cards font14">
    <% foreach (var serviceCenter in ServiceCenteList)
    { %>
    <li class="card" title="<%=serviceCenter.Name %>">
        <div class="card-target">
            <h3 class="text-black text-bold text-truncate margin-bottom5"><%=serviceCenter.Name %></h3>
            <% if (!string.IsNullOrEmpty(serviceCenter.Address)){ %>
            <p class="text-light-grey margin-bottom5">
                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                <span class="vertical-top details-column"><%= Bikewale.Utility.FormatDescription.TruncateDescription(serviceCenter.Address, 80) %></span>
            </p>
            <% } %>
            <% if (!string.IsNullOrEmpty(serviceCenter.Phone)){ %>
            <p class="text-default">
                <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                <span class="text-bold vertical-top details-column text-truncate"><%=serviceCenter.Phone %></span>
            </p>
            <% } %>
        </div>
    </li>
    <% } %>
</ul>
<div class="clear"></div>
<div class="margin-right10 margin-left10 border-solid-bottom"></div>     
<% } %>