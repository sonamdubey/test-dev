<%@ Control Language="C#" AutoEventWireup="true" Inherits="Bikewale.Mobile.Controls.DealersCard" %>
<% if (showWidget)
   { %>
<div class="margin-right20 margin-left20 border-divider"></div>

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
                        <span class="dealership-address vertical-top"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                    </p>
                    <div class=" margin-right20 margin-bottom10 <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"block" %>"><a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class=" margin-bottom5 maskingNumber text-default text-bold"><span class="bwmsprite tel-sm-icon"></span><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></a></div>
                    <div class=" margin-right20 <%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":"block" %>"><a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"Email") %></a></div>
                    <input type="button" data-leadsourceid="<%= LeadSourceId %>" data-pqsourceid="<%= PQSourceId %>" data-item-name="<%# DataBinder.Eval(Container.DataItem,"Name") %>" data-item-area="<%# (DataBinder.Eval(Container.DataItem,"objArea")!=null) ? DataBinder.Eval(Container.DataItem,"objArea.AreaName") : "" %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>"
                        class="margin-top15 btn btn-white font14 leadcapturebtn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers from dealer" />
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul> 
    <a href="/m<%= Bikewale.Utility.UrlFormatter.DealerLocatorUrl(makeMaskingName, cityMaskingName) %>" class="margin-left20">View all dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
</div>
<% } %>
