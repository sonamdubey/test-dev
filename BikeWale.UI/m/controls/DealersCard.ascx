<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.DealersCard" %>
<% if (showWidget)
   {
       %>
<div class="margin-right20 margin-left20 border-divider"></div>
<%
       if (isCitySelected) { 
%>

<div id="dealersInCityWrapper" class="content-inner-block-2017 font14">
    <h2 class="font14 margin-top5 text-dark-black"><%= makeName %> dealers in <%= cityName %></h2>
    <ul class="dealer-in-city-list">
        <asp:Repeater ID="rptDealers" runat="server">
            <ItemTemplate>
                <li>
                    <h3 class="margin-bottom10">
                        <%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
                    </h3>
                    <p class="margin-bottom10">
                        <span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span>
                        <span class="dealership-details vertical-top text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                    </p>
                    <div class="margin-right20 margin-bottom10 <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"block" %>">
                        <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="margin-bottom5 maskingNumber text-default text-bold">
                            <span class="vertical-top bwmsprite tel-sm-icon"></span>
                            <span class="text-default text-bold vertical-top dealership-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></span>
                        </a>
                    </div>
                    <div class="margin-right20 <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":"block" %>">
                        <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey">
                            <span class="vertical-top bwmsprite mail-grey-icon"></span>
                            <span class="vertical-top dealership-details text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Email") %></span>
                        </a>
                    </div>
                    <% if (!IsDiscontinued)
                       { %>
                    <input type="button" data-leadsourceid="<%= LeadSourceId %>" data-pqsourceid="<%= PQSourceId %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# (DataBinder.Eval(Container.DataItem,"objArea")!=null) ? DataBinder.Eval(Container.DataItem,"objArea.AreaName") : "" %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>"
                        class="margin-top15 btn btn-white font14 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers from dealer" />
                    <% } %>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>">View all dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
<% } else { %>

<div class="swiper-container margin-bottom15">
    <!-- dealers when no city selected -->
    <div class="swiper-wrapper">
        <asp:Repeater ID="rptPopularCityDealers" runat="server">
            <ItemTemplate>
                <div class="swiper-slide bike-carousel-swiper dealer-no-city">
                    <a href="<%# String.Format("/m/{0}-bikes/dealers-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>">
                        <span></span>
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
