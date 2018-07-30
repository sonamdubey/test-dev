<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MPriceInTopCities" %>
<% if(showWidget) { %>  
<div id="prices-top-cities">
    <h3 class="padding-top15 padding-right20 padding-left20 margin-bottom20">Price by cities<span class="text-light-grey text-unbold"> (<% if(!IsDiscontinued) { %>On road price<% } else { %>Ex-showroom price<%} %>)</span></h3>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="padding-right20 padding-left20 font14">
        <asp:Repeater ID="rptTopCityPrices" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="left" width="60%" class="city-name padding-bottom20">
                        <a title="<%= string.Format("{0} Price in ",bikeName) %><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %>" href="/m<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "CityMaskingName")) ) %>"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                    </td>
                    <td align="right" width="40%" class="city-price padding-bottom20">
                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %>
                    </td>
                </tr>
            </ItemTemplate>        
        </asp:Repeater>
    </table>
</div>
<% } %>
