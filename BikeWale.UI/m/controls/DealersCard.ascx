<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.DealersCard" %>
<% if (showWidget)
   {
%>
<%
       if (isCitySelected)
       { 
%>
<div id="makeDealersContent" class="bw-model-tabs-data padding-top15 font14">
    <h2 class="padding-right20 padding-left20"><%= makeName %> dealers in India</h2>
    <div class="swiper-container margin-bottom15">
        <!-- dealers by city -->
        <div class="swiper-wrapper">
            <asp:Repeater ID="rptDealers" runat="server">
                <ItemTemplate>
                    <div class="swiper-slide bike-carousel-swiper dealer-by-city">
                        <h4 class="margin-bottom5 text-truncate"><%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %></h4>
                        <p class="margin-bottom5 text-light-grey"><span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span><span class="vertical-top dealership-address"><%# DataBinder.Eval(Container.DataItem,"Address") %></span></p>
                        <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="block margin-bottom5 text-default text-truncate <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"block" %>"><span class="bwmsprite tel-sm-grey-icon"></span><%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></a>
                        <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey block margin-bottom20 <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":"block" %>"><span class="bwmsprite mail-grey-icon vertical-top"></span><span class="text-truncate vertical-top dealership-email"><%# DataBinder.Eval(Container.DataItem,"Email") %></span></a>
                        <% if (!IsDiscontinued)
                           { %>
                        <input type="button" data-leadsourceid="<%= LeadSourceId %>" data-pqsourceid="<%= PQSourceId %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# (DataBinder.Eval(Container.DataItem,"objArea")!=null) ? DataBinder.Eval(Container.DataItem,"objArea.AreaName") : "" %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>"
                            class="margin-top15 btn btn-white font14 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers" />
                        <%} %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="padding-right20 padding-left20">
    <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>">View all dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
<% }
       else
       { %>

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
                        <h4 class="font14 text-bold text-default margin-bottom10"><%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %></h4>
                        <p class="font14 text-black"><%# DataBinder.Eval(Container.DataItem,"NumOfDealers") %> showrooms</p>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<% } %>
<% } %>
