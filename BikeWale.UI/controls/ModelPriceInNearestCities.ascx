<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ModelPriceInNearestCities" %>
<div id="modelPriceInNearbyCities" class="content-inner-block-20">
    <h2 class="font14 text-bold text-x-black margin-bottom15"> <%= model %> price in cities near <%=cityName%> <span class="text-light-grey text-unbold">(<% if(!IsDiscontinued) { %>On road price<% } else { %>Ex-showroom price<%} %>)</span></h2>
    <ul>
        <asp:Repeater ID="rptTopCityPrices" runat="server">
            <ItemTemplate>
                <li>
                    <a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/price-in-<%# DataBinder.Eval(Container.DataItem, "CityMaskingName").ToString() %>/" 
                        title="<%# String.Format("{0} {1}",DataBinder.Eval(Container.DataItem, "Make").ToString(),DataBinder.Eval(Container.DataItem, "Model").ToString()) %> Price in <%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "CityName").ToString() %></a>
                    <span class="nearby-city-price"><span class="bwsprite inr-sm-dark"></span>
                        <%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "OnRoadPrice").ToString()) %></span>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clear"></div>
</div>
