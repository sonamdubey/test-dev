<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.DealersCard" EnableViewState="false" %>
<% if (showWidget)
   {
%>
<%
       if (isCitySelected)
       { 
%>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
    <% if(isHeadingNeeded)  { %>  
    <div class="carousel-heading-content">
        <div class="swiper-heading-left-grid inline-block">
            <h2><%=widgetHeading%></h2>
        </div>
        <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> Showrooms in <%= cityName %>" class="btn view-all-target-btn">View all</a>
        </div>
        <div class="clear"></div>
    </div>
    <% } %>
    <div class="swiper-container card-container dealer-horizontal-swiper">
        <!-- dealers by city -->
        <div class="swiper-wrapper">
           <% foreach (var dealers in dealerList)
            { %> 
            <div class="swiper-slide">
                <div class="swiper-card">
                    <a href = "/m<%= Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName, dealers.Name.ToString(),Convert.ToInt32(dealers.DealerId))%>" title="<%=dealers.Name.ToString()%>" class="block">
                    <%= GetDealerDetailLink(dealers.DealerType.ToString(), dealers.DealerId.ToString(), dealers.CampaignId.ToString(), dealers.Name.ToString()) %>
                        <p class="margin-bottom5 text-light-grey <%= (String.IsNullOrEmpty(dealers.Address.ToString()))?"hide":string.Empty %>">
                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                        <span class="vertical-top dealership-address details-column"><%= Convert.ToString(dealers.Address) %></span>
                        </p>
                    </a>
                    <a href="tel:<%= dealers.MaskingNumber%>" class="text-default text-bold text-truncate <%= (String.IsNullOrEmpty(dealers.MaskingNumber))? "hide":"block" %>"><span class="bwmsprite tel-sm-grey-icon"></span><%= dealers.MaskingNumber %></a>
                </div>
            </div>
            <% } %>
        </div>
    </div>
</div>
<% }
       else
       { %>
<%if(cityDealers!=null){ %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
<h2 class="padding-right20 padding-left20"><%= string.Format("{0} {1} {2} {3}", makeName ,((cityDealers.TotalDealerCount>0)?"Showrooms":""),((cityDealers.TotalDealerCount>0 && cityDealers.TotalServiceCenterCount>0)?"&":""),((cityDealers.TotalServiceCenterCount>0)?"Service Centers":"")) %></h2>
    <div class="swiper-container card-container dealer-horizontal-swiper no-city-selection-carousel">
        <!-- dealers when no city selected -->
        <div class="swiper-wrapper">

            <% foreach(var details in cityDealers.DealerDetails){ %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <span class="dealer-city-image-preview">
                            <span class="city-sprite <%=details.CityMaskingName %>-icon"></span>
                        </span>
                        <p class="text-default text-bold margin-bottom5"><%= makeName %> <%=(details.DealerCount > 1 )? " showrooms" : " showroom" %> in <%=details.CityName %></p>
                        <%if (details.DealerCount>0) {%>
                        <a href="/m/dealer-showrooms/<%=makeMaskingName%>/<%=details.CityMaskingName%>/" title="<%=makeName%> showroom in <%=details.CityName%>" class="block"><%=details.DealerCount %><%=(details.DealerCount > 1 )? " showrooms" : " showroom" %></a>
                        <%} %>
                        <%if (details.ServiceCenterCount>0){%>
                        <a href="/m/service-centers/<%=makeMaskingName%>/<%=details.CityMaskingName%>/" title="<%=makeName%> service center in <%=details.CityName%>" class="block"><%=details.ServiceCenterCount %> service center<%=(details.ServiceCenterCount > 1 )? "s" : "" %></a>
                        <%} %>
                    </div>
                </div>
                <%} %>
                <%if(cityDealers.TotalDealerCount>0||cityDealers.TotalServiceCenterCount>0) {%>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <span class="dealer-city-image-preview">
                            <span class="city-sprite india-icon"></span>
                        </span>
                        <p class="text-default text-bold margin-bottom5"><%= makeName %> <%=(cityDealers.TotalDealerCount > 1 )? " showrooms" : " showroom" %> in India</p>
                        <%if (cityDealers.TotalDealerCount > 0) {%>
                        <a href="/m/dealer-showrooms/<%=makeMaskingName%>/" title="<%=makeName%> showroom in India" class="block"><%=cityDealers.TotalDealerCount %><%=(cityDealers.TotalDealerCount > 1 )? " showrooms" : " showroom" %></a>
                        <%} %>
                        <%if (cityDealers.TotalServiceCenterCount > 0) {%>
                        <a href="/m/service-centers/<%=makeMaskingName%>/" title="<%=makeName%> service center in India" class="block"><%=cityDealers.TotalServiceCenterCount %> service center<%=(cityDealers.TotalServiceCenterCount > 1 )? "s" : "" %></a>
                        <%} %>
                    </div>
                </div>
             <%} %>
             
        </div>
     </div>
</div>
<%} %>
<% } %>
<% } %>
