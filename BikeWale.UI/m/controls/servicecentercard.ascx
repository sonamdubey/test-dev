<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCenterCard" %>
<% if (showWidget && ServiceCenteList!= null)
   { %>
<div class="container card-bottom-margin padding-bottom20">
    <div class="carousel-heading-content">
        <div class="swiper-heading-left-grid inline-block">
            <h2><%=widgetHeading %></h2>
        </div>
        <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> Service Centers in <%=cityName %>" class="btn view-all-target-btn">View all</a>
        </div>
        <div class="clear"></div>
    </div>
    <%if(!string.IsNullOrEmpty(biLineText)) {%>
    <p class="margin-top5 margin-left20 margin-bottom15"><%=biLineText %></p>
    <%} %>
    <div class="swiper-container card-container margin-bottom15 dealer-horizontal-swiper font14">
        <div class="swiper-wrapper">
            <% foreach (var serviceCenter in ServiceCenteList)
               { %>
            <div class="swiper-slide">
                <div class="swiper-card">
                    <div class="block">
                        <div class="target-link margin-bottom5 text-truncate font14"><%= serviceCenter.Name%></div>
                        <% if (!string.IsNullOrEmpty(serviceCenter.Address))
                           { %>
                        <p class="margin-bottom5 text-light-grey">
                            <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                            <span class="vertical-top dealership-address details-column"><%= Bikewale.Utility.FormatDescription.TruncateDescription(serviceCenter.Address, 95) %></span>
                        </p>
                        <% } %>
                    </div>
                    <% if (!string.IsNullOrEmpty(serviceCenter.Phone))
                    { %>
                    <a href="tel:<%=serviceCenter.Phone %>" class="text-default text-bold text-truncate block">
                        <span class="bwmsprite tel-sm-grey-icon"></span><%=serviceCenter.Phone %>
                    </a>
                    <% } %>
                </div>
            </div>
            <% } %>
        </div>
    </div>
</div>
<% } %>
