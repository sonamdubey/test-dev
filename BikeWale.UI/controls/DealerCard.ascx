<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.DealerCard" %>
<% if (showWidget)
   { %>
<div class="margin-right20 margin-left20 border-divider"></div>
<% if (isCitySelected)
   { %>
<div id="makeDealersContent" class="padding-top20 padding-bottom20 border-solid-bottom">
    <h2 class="font15 text-bold text-x-black padding-right20 padding-left20"><%=makeName %> dealers in <%=cityName %></h2>
    <div class="grid-12 padding-top15">
        <ul>
            <asp:Repeater ID="rptDealers" runat="server">
                <ItemTemplate>
                    <li class="dealer-details-item grid-4 margin-bottom20">
                        <h3 class="font14"><a href="<%# Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName, Convert.ToString(DataBinder.Eval(Container.DataItem,"DealerId"))) %>" class="text-default"><%# DataBinder.Eval(Container.DataItem,"Name") %></a></h3>
                        <div class="margin-top10">
                            <p class="text-light-grey margin-bottom5">
                                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                <span class="vertical-top dealership-card-details">
                                    <span class="dealer-details-main-content" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")) %>"><%# Bikewale.Utility.FormatDescription.TruncateDescription(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")), 75) %></span>
                                    <span class="dealer-details-more-content"><%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")) %></span>
                                </span>
                            </p>
                            <p class="margin-bottom5 <%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingNumber"))))?"hide":string.Empty %>">
                                <span class="text-bold">
                                    <span class="bwsprite phone-black-icon vertical-top"></span>
                                    <span class="vertical-top dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                                </span>
                            </p>
                            <p class="margin-bottom15 <%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"Email"))))?"hide":string.Empty %>">
                                <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey">
                                    <span class="bwsprite mail-grey-icon vertical-top"></span>
                                    <span class="vertical-top dealership-card-details"><%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Email")).Replace(",",", ") %></span>
                                </a>
                            </p>
                            <% if (!IsDiscontinued)
                               { %>
                            <input type="button" data-leadsourceid="<%= LeadSourceId %>" data-pqsourceid="<%= PQSourceId %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# (DataBinder.Eval(Container.DataItem,"objArea")!=null) ? DataBinder.Eval(Container.DataItem,"objArea.AreaName") : "" %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>"
                                data-camp-id="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>" class="btn btn-grey btn-md font14 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers from dealer" />
                            <%} %>
                        </div>
                        <div class="clear"></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <div class="clear"></div>
    <a href="<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" class="margin-left20">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
</div>
<% }
   else
   { %>
<div id="makeDealersContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
    <h2 class="padding-left10 padding-right10"><%= makeName %> Dealers in India</h2>
    <div class="jcarousel-wrapper bike-carousel-wrapper margin-bottom15">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptPopularCityDealers" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="<%# String.Format("/{0}-bikes/dealers-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>" class="dealer-jcarousel-image-preview">
                                <span class="city-sprite <%# DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName") %>-icon"></span>
                            </a>
                            <h3 class="margin-bottom10"><a href="<%# String.Format("/{0}-bikes/dealers-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>" class="text-default"><%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %></a></h3>
                            <p><%# DataBinder.Eval(Container.DataItem,"NumOfDealers") %> <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"NumOfDealers")) > 1 ? "showrooms" : "showroom" %></p>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
    <div class="padding-left10">
        <a href="/new/<%= makeMaskingName %>-dealers/">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
</div>
<% } %>
<% } %>
