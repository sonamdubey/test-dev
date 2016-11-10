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
    <h2 class="padding-right20 padding-left20 text-bold"><%= makeName %> dealers in <%= cityName %></h2>
    <% } %>
    <div class="swiper-container card-container margin-bottom15">
        <!-- dealers by city -->
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptDealers" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide">
                        <div class="swiper-card">
                        <a href = "/m<%# Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName, DataBinder.Eval(Container.DataItem,"Name").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"DealerId"))) %>">
                                <%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %>

                                <p class="margin-bottom5 text-light-grey <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":string.Empty %>">
                                    <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top dealership-address"><%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")) %></span>
                                </p>
                            </a>
                            <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="text-default text-bold text-truncate <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"block" %>"><span class="bwmsprite tel-sm-grey-icon pos-top0 margin-right5"></span><%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></a>
                        </div>
                    </div>                        
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <div class="padding-right20 padding-left20">
        <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> Dealer showrooms in <%= cityName %>">View all <%=makeName %> dealer showrooms <span class="bwmsprite blue-right-arrow-icon font14"></span></a>
    </div>
</div>
<script>
    $('.dealer-details-main-content').on('click', function () { $(this).hide(); $(this).next('.dealer-details-more-content').show(); });      
</script>
<% }
       else
       { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
<h2 class="padding-right20 padding-left20"><%= makeName %> dealers in India</h2>

    <div class="swiper-container card-container margin-bottom15">
        <!-- dealers when no city selected -->
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptPopularCityDealers" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide no-city-dealer-list">
                        <div class="swiper-card">
                            <a href="<%# String.Format("/m/{0}-dealer-showrooms-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>" class="font14">
                                <span class="dealer-city-image-preview">
                                    <span class="city-sprite <%# DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName") %>-icon"></span>
                                </span>
                                <p class="text-bold text-default margin-bottom5"><%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %></p>
                                <p class="text-light-grey"><%# DataBinder.Eval(Container.DataItem,"NumOfDealers") %> <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"NumOfDealers")) > 1 ? "showrooms" : "showroom" %></p>
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
     </div>
     <div class="padding-right20 padding-left20">
        <a href="<%= String.Format("/m/new/{0}-dealers/", makeMaskingName) %>"title="<%=makeName %> Dealers in India">View all dealers in India<span class="bwmsprite blue-right-arrow-icon font14"></span></a>
     </div>
</div>
<% } %>
<% } %>
