<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.DealerCard" %>
<% if (showWidget)
   { %>
<% if (isCitySelected)
   { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-top20 padding-bottom20 border-solid-bottom font14">
    <h2 class="font15 text-bold text-x-black padding-right20 padding-left20"><%=makeName %> dealers in <%=cityName %></h2>
    <div class="grid-12">
        <ul id="city-dealer-list">
            <asp:Repeater ID="rptDealers" runat="server">
                <ItemTemplate>
                    <li class="dealer-details-item margin-bottom20">
                        <a href="<%# Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName, Convert.ToString(DataBinder.Eval(Container.DataItem,"DealerId"))) %>" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="font14 text-default">
                            <p class="text-black text-bold text-truncate margin-bottom5"><%# DataBinder.Eval(Container.DataItem,"Name") %></p>
                            <p class="<%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address"))))?"hide": "text-light-grey margin-bottom5" %>">
                                <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                <span class="vertical-top dealership-card-details"><%# Bikewale.Utility.FormatDescription.TruncateDescription(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")), 95) %></span>
                            </p>
                            <p class="<%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingNumber"))))?"hide":string.Empty %>">
                                <span class="bwsprite phone-black-icon vertical-top"></span>
                                <span class="text-bold vertical-top dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                            </p>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <div class="clear"></div>
    <a href="<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> Dealers" class="margin-left20">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
</div>
<% }
   else
   { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-top20 padding-bottom20 border-solid-bottom font14">
    <h2 class="padding-left20 padding-right20"><%= makeName %> Dealers in India</h2>
    <div class="jcarousel-wrapper inner-content-carousel margin-bottom15">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptPopularCityDealers" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="<%# String.Format("/{0}-bikes/dealers-in-{1}/", makeMaskingName ,DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName")) %>" title="<%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %>" class="dealer-card-target">
                                <div class="dealer-jcarousel-image-preview">
                                    <span class="city-sprite <%# DataBinder.Eval(Container.DataItem,"CityBase.CityMaskingName") %>-icon"></span>
                                </div>
                                <div class="font14 padding-left20 padding-right20 padding-bottom25">
                                    <p class="text-default text-bold margin-bottom5"><%= makeName %> dealers in <%# DataBinder.Eval(Container.DataItem,"CityBase.CityName") %></p>
                                    <p class="text-light-grey"><%# DataBinder.Eval(Container.DataItem,"NumOfDealers") %> <%# Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"NumOfDealers")) > 1 ? "showrooms" : "showroom" %></p>
                                </div>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
    <div class="padding-left20">
        <a href="/new/<%= makeMaskingName %>-dealers/" title="<%=makeName %> Dealers">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
</div>
<% } %>
<% } %>
 <script type="text/javascript">
 

     var bikeCity='<%=makeName%> '+'_'+'<%=cityName%>'
     </script>