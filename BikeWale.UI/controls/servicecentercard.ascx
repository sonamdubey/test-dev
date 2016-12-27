<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceCenterCard" %>
<% if(ServiceCenteList!=null) { %>

<h2 class="padding-top15 padding-right20 margin-bottom15 padding-left20"><%=widgetHeading%></h2>
<%if(!string.IsNullOrEmpty(biLineText)) {%>
<p class="font14 margin-left20 margin-bottom20"><%=biLineText%></p>
<%} %>
<ul class="bw-horizontal-cards font14">
    <% foreach (var serviceCenter in ServiceCenteList)
    { %>
    <li class="card">
        <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,cityMaskingName, serviceCenter.Name,serviceCenter.ServiceCenterId) %>" title="<%=serviceCenter.Name %>" class="card-target">
            <h3 class="text-black text-bold text-truncate margin-bottom5"><%=serviceCenter.Name %></h3>
            <% if (!string.IsNullOrEmpty(serviceCenter.Address)){ %>
            <p class="text-light-grey margin-bottom5">
                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                <span class="vertical-top details-column text-truncate"><%=serviceCenter.Address %></span>
            </p>
            <% } %>
            <% if (!string.IsNullOrEmpty(serviceCenter.Phone)){ %>
            <p class="text-default">
                <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                <span class="text-bold vertical-top details-column text-truncate"><%=serviceCenter.Phone %></span>
            </p>
            <% } %>
        </a>
    </li>
    <% } %>
</ul>
<div class="clear"></div>
<div class="padding-left20 font14 padding-bottom20">
    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName) %>" title="<%= makeName %> service centers in <%=cityName %>" >View all <%= makeName %> service centers in <%=cityName %><span class="bwsprite blue-right-arrow-icon"></span></a>
</div>
<div class="margin-right10 margin-left10 border-solid-bottom"></div>     
<% } %>