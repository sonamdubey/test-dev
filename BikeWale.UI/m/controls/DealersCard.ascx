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
    <h2 class="padding-right20 padding-left20 text-bold"><%=widgetHeading%></h2>
    <% } %>
    <div class="swiper-container card-container margin-bottom15">
        <!-- dealers by city -->
        <div class="swiper-wrapper">
           <% foreach (var dealers in dealerList)
                       { %> 
                        <div class="swiper-slide">
                            <div class="swiper-card">
                               <a href = "/m<%= Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName, dealers.Name.ToString(),Convert.ToInt32(dealers.DealerId))%>" title="<%=dealers.Name.ToString()%>"> 
                               <%= GetDealerDetailLink(dealers.DealerType.ToString(), dealers.DealerId.ToString(), dealers.CampaignId.ToString(), dealers.Name.ToString()) %>
                                 <p class="margin-bottom5 text-light-grey <%= (String.IsNullOrEmpty(dealers.Address.ToString()))?"hide":string.Empty %>">
                                    <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top dealership-address details-column"><%= Convert.ToString(dealers.Address) %></span>
                                </p>
                            </a>
                                <a href="tel:<%= dealers.MaskingNumber%>" class="text-default text-bold text-truncate <%= (String.IsNullOrEmpty(dealers.MaskingNumber))? "hide":"block" %>"><span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span><%= dealers.MaskingNumber %></a>
                            </div>
                        </div>
            <% } %>
        </div>
    </div>

    <div class="padding-right20 padding-left20">
        <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> showroom in <%= cityName %>">View all <%=makeName %> showrooms <span class="bwmsprite blue-right-arrow-icon font14"></span></a>
    </div>
</div>
<% }
       else
       { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
<h2 class="padding-right20 padding-left20"><%= makeName %> Showrooms & Service Centers</h2>

    <div class="swiper-container card-container margin-bottom15">
        <!-- dealers when no city selected -->
        <div class="swiper-wrapper">

            <% foreach(var details in cityDealers.DealerDetails){ %>
                    <div class="swiper-slide no-city-dealer-list">
                        <div class="swiper-card">
                               <span class="dealer-city-image-preview">
                                    <span class="city-sprite <%=details.CityMaskingName %>-icon"></span>
                                </span>
                                <p class="text-default text-bold margin-bottom5"><%= makeName %> outlets in <%=details.CityName %></p>
                                    <a href="/m/<%=makeMaskingName%>-dealer-showrooms-in-<%=details.CityMaskingName%>/" ><%=details.DealerCount %> <%=(details.DealerCount > 1 )? "showrooms" : "showroom" %></a>
                                    <p><a href="/m/<%=makeMaskingName%>-service-center-in-<%=details.CityMaskingName%>/"><%=details.ServiceCenterCount %> Service Center<%=(details.ServiceCenterCount > 1 )? "s" : "" %></a></p>
                                 </div>
                    </div>
                <%} %>
             
        </div>
     </div>
     <div class="padding-right20 padding-left20">
        <a href="<%= String.Format("/m/new/{0}-dealers/", makeMaskingName) %>"title="<%=makeName %> Dealers in India">View all dealers in India<span class="bwmsprite blue-right-arrow-icon font14"></span></a>
     </div>
</div>
<% } %>
<% } %>
