<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCenterCard" %>
<% if (showWidget)
   { %>
<div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
    <h2 class="padding-15-20"><%= widgetTitle %></h2>
    <div class="bw-horizontal-swiper swiper-container card-container margin-bottom15">
        <div class="swiper-wrapper">
            <% foreach (var serviceCenter in ServiceCenteList)
               { %>
            <div class="swiper-slide">
                <div class="swiper-card">
                    <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,cityMaskingName, serviceCenter.Name,(int)serviceCenter.ServiceCenterId) %>" title="<%= serviceCenter.Name%>">
                        <div class="target-link margin-bottom5 text-truncate font14"><%= serviceCenter.Name%></div>
                        <% if (!string.IsNullOrEmpty(serviceCenter.Address))
                           { %>
                        <p class="margin-bottom5 text-light-grey">
                            <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                            <span class="vertical-top details-column"><%= Bikewale.Utility.FormatDescription.TruncateDescription(serviceCenter.Address, 95) %></span>
                        </p>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(serviceCenter.Phone))
                           { %>
                        <p class="text-truncate">
                            <span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span>
                            <span class="text-bold text-default"><%=serviceCenter.Phone %></span>
                        </p>
                        <% } %>
                    </a>
                </div>
            </div>
            <% } %>
        </div>
    </div>

<div class="padding-right20 padding-left20 font14">
    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> service centers in <%=cityName %>">View all <%= makeName %> service centers <span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
    </div>
<% } %>
