<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.DealerCard" %>
<% if (showWidget)
   { %>
<% if (isCitySelected)
   { %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
    <% if(isHeading) { %>
    <h2 class="font15 text-bold text-x-black padding-right20 padding-left20 margin-bottom15"><%=widgetHeading %></h2>
    <% } %>
    <ul id="city-dealer-list" class="bw-horizontal-cards">
        <asp:Repeater ID="rptDealers" runat="server">
            <ItemTemplate>
                <li class="card">
                    <a href="<%# Bikewale.Utility.UrlFormatter.GetDealerUrl(makeMaskingName, cityMaskingName,DataBinder.Eval(Container.DataItem,"Name").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"DealerId"))) %>" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="card-target">
                        <p class="text-black text-bold text-truncate margin-bottom5"><%# DataBinder.Eval(Container.DataItem,"Name") %></p>
                        <p class="<%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address"))))?"hide": "text-light-grey margin-bottom5" %>">
                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                            <span class="vertical-top details-column"><%# Bikewale.Utility.FormatDescription.TruncateDescription(Convert.ToString(DataBinder.Eval(Container.DataItem,"Address")), 95) %></span>
                        </p>
                        <p class="<%# (String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(Container.DataItem,"MaskingNumber"))))?"hide": "text-default" %>">
                            <span class="bwsprite phone-black-icon vertical-top"></span>
                            <span class="text-bold vertical-top details-column text-truncate"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                        </p>
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clear"></div>
    <a href="<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" title="<%=makeName %> showroom in <%=cityName %>" class="margin-left20">View all <%=makeName %> showrooms<span class="bwsprite blue-right-arrow-icon"></span></a>
</div>
<div class="margin-right10 margin-left10 border-solid-bottom"></div>
<% }
   else
   { %>
<%if(cityDealers!=null){ %>
<div id="makeDealersContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
    <h2 class="padding-left20 padding-right20"><%= makeName %> <%=(cityDealers.TotalDealerCount>0)?"Showrooms":"" %> <%=(cityDealers.TotalDealerCount>0 && cityDealers.TotalServiceCenterCount>0)?"&":"" %> <%=(cityDealers.TotalServiceCenterCount>0)?"Service Centers":"" %></h2>
    <div class="jcarousel-wrapper inner-content-carousel no-city-selection-carousel">
        <div class="jcarousel">
            <ul>
                <% foreach(var details in cityDealers.DealerDetails){ %>
                    <li>
                        <div class="dealer-jcarousel-image-preview">
                            <span class="city-sprite <%=details.CityMaskingName %>-icon"></span>
                        </div>
                        <div class="font14 padding-left20 padding-right20 padding-bottom20">
                            <p class="text-default text-bold margin-bottom5"><%= makeName %> outlets in <%=details.CityName %></p>
                            <%if (details.DealerCount>0) {%>
                            <a href="/<%=makeMaskingName%>-dealer-showrooms-in-<%=details.CityMaskingName%>/" title="<%=makeName%> showroom in <%=details.CityName%>" class="block"><%=details.DealerCount %> <%=(details.DealerCount > 1 )? "showrooms" : "showroom" %></a>
                            <%} %>
                            <%if (details.ServiceCenterCount>0){%>
                            <a href="/<%=makeMaskingName%>-service-center-in-<%=details.CityMaskingName%>/" title="<%=makeName%> service center in <%=details.CityName%>" class="block"><%=details.ServiceCenterCount %> service center<%=(details.ServiceCenterCount > 1 )? "s" : "" %></a>
                            <%} %>
                        </div>
                    </li>
                <%} %>
               <%if(cityDealers.TotalDealerCount>0||cityDealers.TotalServiceCenterCount>0) {%>
                    <li>
                        <div class="dealer-jcarousel-image-preview">
                            <span class="city-sprite india-icon"></span>
                        </div>
                        <div class="font14 padding-left20 padding-right20 padding-bottom20">
                            <p class="text-default text-bold margin-bottom5"><%= makeName %> outlets in India</p>
                            <%if (cityDealers.TotalDealerCount > 0)
                            { %>
                            <a href="/<%=makeMaskingName%>-dealer-showrooms-in-india/" title="<%=makeName%> showroom in India" class="block"><%=cityDealers.TotalDealerCount %> <%=(cityDealers.TotalDealerCount>0 )? "showrooms" : "showroom" %></a>
                            <%} %>
                            <%if (cityDealers.TotalServiceCenterCount > 0)
                            { %>
                            <a href="/<%=makeMaskingName%>-service-center-in-india/" title="<%=makeName%> service center in India" class="block"><%=cityDealers.TotalServiceCenterCount %> service center<%=(cityDealers.TotalServiceCenterCount > 1 )? "s" : "" %></a>
                                    <%} %>
                                </div>
                        </li>
                <%} %>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
</div>
<div class="margin-right10 margin-left10 border-solid-bottom"></div>
<% } %>
<% } %>
<% } %>
 <script type="text/javascript">
     var bikeCity = '<%=makeName%> ' + '_' + '<%=cityName%>'
</script>