﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ModelPriceInNearestCities" %>
<% if(showWidget) { %>

<div class="margin-right20 margin-left20 border-divider"></div>
<div id="modelPriceInNearbyCities" class="padding-top20 padding-right20 padding-bottom10 padding-left20 font14">
    <h2 class="font14 text-bold text-dark-black margin-bottom15">Prices in nearby cities</h2>
    <ul class="prices-by-cities-list">
         <asp:Repeater ID="rptTopCityPrices" runat="server">
            <ItemTemplate>
                <li>
                    <a class="text-truncate" href="/m/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/price-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/" title="Price in <%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                    <span class="nearby-city-price">
                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span>&nbsp;<%# Bikewale.Utility.Format.FormatPriceShort(DataBinder.Eval(Container.DataItem, "OnRoadPrice").ToString()) %>  
                    </span>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
<% } %>
