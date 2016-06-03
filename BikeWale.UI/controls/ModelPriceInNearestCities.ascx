<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ModelPriceInNearestCities" %>
<% if(showWidget) { %>
<div class="margin-right20 margin-left20 border-divider"></div>
<div id="modelPriceInNearbyCities" class="content-inner-block-20">
    <h2 class="font14 text-bold text-x-black margin-bottom15"><%= make %> <%= model %> price in nearby cities <span class="text-light-grey text-unbold">(On road price)</span></h2>
    <ul>
        <asp:Repeater ID="rptTopCityPrices" runat="server">
            <ItemTemplate>
                <li>
                    <a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/price-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/" title="Price in <%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                    <span class="nearby-city-price"><span class="fa fa-rupee"></span>&nbsp;<%# Bikewale.Utility.Format.FormatPriceShort(DataBinder.Eval(Container.DataItem, "OnRoadPrice").ToString()) %></span>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clear"></div>
</div>
<% } %>