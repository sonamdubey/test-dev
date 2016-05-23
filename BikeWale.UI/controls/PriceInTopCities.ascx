<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PriceInTopCities" %>
<div class="grid-4 omega padding-left20">
    <h3>Prices by cities<span class="text-light-grey text-unbold"> (Ex-showroom)</span></h3>
     <ul class="prices-by-cities-list font14">
    <asp:Repeater ID="rptTopCityPrices" runat="server">
        <ItemTemplate>       
            <li>
                <div class="grid-7 alpha">
                    <a href="javascript:void(0)" class="text-truncate"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                </div>
                <div class="grid-5 alpha omega text-right">
                    <span class="fa fa-rupee"></span>
                    <span><%# Bikewale.Utility.Format.FormatPriceShort(DataBinder.Eval(Container.DataItem,"OnRoadPrice").ToString()) %></span>
                </div>
                <div class="clear"></div>
            </li>
        </ItemTemplate>        
    </asp:Repeater>
    </ul>
</div>