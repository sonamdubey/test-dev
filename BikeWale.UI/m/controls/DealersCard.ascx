<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.DealersCard" EnableViewState="false" %>
<% if (showWidget)
   {
%>
<%
       if (isCitySelected)
       { 
%>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
    <h2 class="padding-right20 padding-left20"><%= makeName %> dealers in <%= cityName %></h2>
    <div class="swiper-container margin-bottom15">
        <!-- dealers by city -->
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptDealers" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide bike-carousel-swiper dealer-by-city">
                        <%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
                        <p class="margin-bottom5 text-light-grey <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":string.Empty %>">
                            <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                            <span class="vertical-top dealership-address dealer-details-main-content"><%# Bikewale.Utility.FormatDescription.TruncateDescription(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")), 60) %></span>
                            <span class="vertical-top dealership-address dealer-details-more-content" style="display:none"><%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")) %></span>
                        </p>
                        <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="margin-bottom5 text-default text-bold text-truncate <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"block" %>"><span class="bwmsprite tel-sm-grey-icon"></span><%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></a>
                        <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":"block" %>"><span class="bwmsprite mail-grey-icon vertical-top"></span><span class="text-truncate vertical-top dealership-email"><%# DataBinder.Eval(Container.DataItem,"Email") %></span></a>
                        <% if (!IsDiscontinued)
                           { %>
                        <input type="button" data-leadsourceid="<%= LeadSourceId %>" data-pqsourceid="<%= PQSourceId %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# (DataBinder.Eval(Container.DataItem,"objArea")!=null) ? DataBinder.Eval(Container.DataItem,"objArea.AreaName") : "" %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>"
                            data-camp-id="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" class="margin-top15 btn btn-white font14 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers" />
                        <%} %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <div class="padding-right20 padding-left20">
        <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>">View all dealers<span class="bwmsprite blue-right-arrow-icon font14"></span></a>
    </div>
</div>
<script>$('.dealer-details-main-content').on('click', function () { $(this).hide(); $(this).next('.dealer-details-more-content').show(); });</script>
<% }
       else
       { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-bottom20 padding-top15 font14">
<h2 class="padding-right20 padding-left20"><%= makeName %> dealers in India</h2>

    <div class="swiper-container margin-bottom15">
        <!-- dealers when no city selected -->
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptPopularCityDealers" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide bike-carousel-swiper dealer-no-city">
                        <a href="<%# String.Format("/m/{0}-bikes/dealers-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>">
                            <span class="dealer-city-image-preview">
                                <span class="city-sprite <%# DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName") %>-icon"></span>
                            </span>
                            <p class="font14 text-bold text-default margin-bottom10"><%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %></p>
                            <p class="font14 text-black"><%# DataBinder.Eval(Container.DataItem,"NumOfDealers") %> <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"NumOfDealers")) > 1 ? "showrooms" : "showroom" %></p>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
     </div>
     <div class="padding-right20 padding-left20">
        <a href="<%= String.Format("/m/new/{0}-dealers/", makeMaskingName) %>">View all dealers<span class="bwmsprite blue-right-arrow-icon font14"></span></a>
     </div>
</div>
<% } %>
<% } %>
