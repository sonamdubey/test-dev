<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PriceInTopCities" %>
<% if(showWidget) { %>  
<div id="prices-top-cities" class="grid-6 omega">
    <h3>Price by cities&nbsp;<span class="text-light-grey text-unbold">(<% if(!IsDiscontinued) { %>On road price<% } else { %>Ex-showroom price<%} %>)</span></h3>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <asp:Repeater ID="rptTopCityPrices" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="city-name padding-bottom20">
                        <a title="<%= string.Format("{0} Price in ",bikeName) %><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %>" href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/price-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/" class="text-truncate"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                    </td>
                    <td align="right" class="city-price padding-bottom20 text-black">
                        <span class="bwsprite inr-sm-dark"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %>
                    </td>
                </tr>
            </ItemTemplate>        
        </asp:Repeater>
    </table>
</div>
<% } %>