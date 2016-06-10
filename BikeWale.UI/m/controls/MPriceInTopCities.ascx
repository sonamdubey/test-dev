<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.controls.MPriceInTopCities" %>
<% if(showWidget) { %>  
<div>
    <h3 class="padding-top15 padding-right20 padding-left20 margin-bottom20">Prices by cities<span class="text-light-grey text-unbold"> (<% if(!IsDiscontinued) { %>On road price<% } else { %>Ex-showroom price<%} %>)</span></h3>
    <ul class="prices-by-cities-list font14">
    <asp:Repeater ID="rptTopCityPrices" runat="server">
        <ItemTemplate>       
            <li>                
                <a href="/m<%# Bikewale.Utility.UrlFormatter.PriceInCityUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "MakeMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelMaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem, "CityMaskingName")) ) %>" class="text-truncate"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>               
                   <span class="price-in-city-price">
                     <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                    <span><%# Bikewale.Utility.Format.FormatPriceShort(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %></span>
                  </span>
            </li>
        </ItemTemplate>        
    </asp:Repeater>
    </ul>
</div>
<% } %>
